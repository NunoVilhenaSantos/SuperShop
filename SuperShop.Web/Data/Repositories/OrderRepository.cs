using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuperShop.Web.Data.DataContext;
using SuperShop.Web.Data.Entities;
using SuperShop.Web.Helpers;
using SuperShop.Web.Models;

namespace SuperShop.Web.Data.Repositories;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    private readonly DataContextMssql _dataContextMssql;
    private readonly IUserHelper _userHelper;


    public OrderRepository(
        IUserHelper userHelper,
        DataContextMssql dataContextMssql
    ) : base(dataContextMssql)
    {
        _userHelper = userHelper;
        _dataContextMssql = dataContextMssql;
    }


    public async Task<IQueryable<Order>> GetOrdersAsync(string userName)
    {
        var user = await _userHelper.GetUserByEmailAsync(userName);


        if (user == null)
            return Array.Empty<Order>().AsQueryable();


        return await _userHelper.IsUserInRoleAsync(user, "Admin")
            ? _dataContextMssql.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .OrderByDescending(o => o.OrderDate)
            : _dataContextMssql.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.User == user)
                .OrderByDescending(o => o.OrderDate);
    }


    public async Task<IQueryable<OrderDetailTemp>> GetDetailsTempAsync(
        string userName)
    {
        var user = await _userHelper.GetUserByEmailAsync(userName);


        if (user == null)
            return Array.Empty<OrderDetailTemp>().AsQueryable();


        return await _userHelper.IsUserInRoleAsync(user, "Admin")
            ? _dataContextMssql.OrderDetailTemps
                .Include(o => o.User)
                .Include(p => p.Product)
                .OrderByDescending(o => o.Product.Name)
            : _dataContextMssql.OrderDetailTemps
                .Include(o => o.Product)
                .Where(o => o.User == user)
                .OrderByDescending(o => o.Product.Name);
    }


    public async Task AddItemToOrderAsync(
        AddItemViewModel model, string userName)
    {
        var user = await _userHelper.GetUserByEmailAsync(userName);

        if (user == null) return;

        var product =
            await _dataContextMssql.Products.FindAsync(model.ProductId);

        if (product == null) return;

        var orderDetailTemp =
            await _dataContextMssql.OrderDetailTemps
                .Where(odt =>
                    odt.User == user &&
                    odt.Product == product)
                .FirstOrDefaultAsync();

        if (orderDetailTemp == null)
        {
            orderDetailTemp = new OrderDetailTemp
            {
                Price = product.Price,
                Product = product,
                Quantity = model.Quantity,
                User = user
            };

            _dataContextMssql.OrderDetailTemps.Add(orderDetailTemp);
        }
        else
        {
            orderDetailTemp.Quantity += model.Quantity;
            _dataContextMssql.OrderDetailTemps.Update(orderDetailTemp);
        }

        await _dataContextMssql.SaveChangesAsync();
    }


    public async Task ModifyOrderDetailTempQuantityAsync(
        int id, double quantity)
    {
        var orderDetailTemp =
            await _dataContextMssql.OrderDetailTemps.FindAsync(id);

        if (orderDetailTemp == null) return;

        orderDetailTemp.Quantity += quantity;

        if (orderDetailTemp.Quantity > 0)
            _dataContextMssql.OrderDetailTemps.Update(orderDetailTemp);
        else
            _dataContextMssql.OrderDetailTemps.Remove(orderDetailTemp);

        await _dataContextMssql.SaveChangesAsync();
    }


    public async Task DeleteDetailTempAsync(int id)
    {
        var orderDetailTemp =
            await _dataContextMssql.OrderDetailTemps.FindAsync(id);

        if (orderDetailTemp == null) return;

        _dataContextMssql.OrderDetailTemps.Remove(orderDetailTemp);

        await _dataContextMssql.SaveChangesAsync();
    }


    public async Task<bool> ConfirmOrderAsync(string userName)
    {
        var user = await _userHelper.GetUserByEmailAsync(userName);

        if (user == null) return false;

        var orderDetailsTemp =
            await _dataContextMssql.OrderDetailTemps
                .Include(odt => odt.Product)
                .Where(odt => odt.User == user)
                .ToListAsync();

        if (orderDetailsTemp.Count == 0) return false;

        var order = new Order
        {
            OrderDate = DateTime.UtcNow,
            User = user,
            Items = orderDetailsTemp.Select(odt => new OrderDetail
            {
                Price = odt.Product.Price,
                Product = odt.Product,
                Quantity = odt.Quantity
            }).ToList()
        };

        // _dataContextMssql.Orders.Add(order);
        await CreateAsync(order);

        _dataContextMssql.OrderDetailTemps.RemoveRange(orderDetailsTemp);

        await _dataContextMssql.SaveChangesAsync();

        return true;
    }
}