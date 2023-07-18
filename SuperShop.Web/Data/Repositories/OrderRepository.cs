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
    private readonly DataContextMsSql _dataContextMsSql;
    private readonly IUserHelper _userHelper;


    public OrderRepository(
        IUserHelper userHelper,
        DataContextMsSql dataContextMsSql,
        DataContextMySql dataContextMySql,
        DataContextSqLite dataContextSqLite
    ) : base(dataContextMsSql, dataContextMySql, dataContextSqLite)
    {
        _userHelper = userHelper;
        _dataContextMsSql = dataContextMsSql;
    }


    public async Task<Order> GetOrdersAsync(int id)
    {
        return await _dataContextMsSql.Orders.FindAsync(id);
    }


    public async Task<IQueryable<Order>> GetOrdersAsync(string userName)
    {
        var user = await _userHelper.GetUserByEmailAsync(userName);


        if (user == null)
            return Array.Empty<Order>().AsQueryable();


        return await _userHelper.IsUserInRoleAsync(user, "Admin")
            ? _dataContextMsSql.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .OrderByDescending(o => o.OrderDate)
            : _dataContextMsSql.Orders
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
            ? _dataContextMsSql.OrderDetailTemps
                .Include(o => o.User)
                .Include(p => p.Product)
                .OrderByDescending(o => o.Product.Name)
            : _dataContextMsSql.OrderDetailTemps
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
            await _dataContextMsSql.Products.FindAsync(model.ProductId);

        if (product == null) return;

        var orderDetailTemp =
            await _dataContextMsSql.OrderDetailTemps
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
                User = user,
                WasDeleted = false
            };

            _dataContextMsSql.OrderDetailTemps.Add(orderDetailTemp);
        }
        else
        {
            orderDetailTemp.Quantity += model.Quantity;
            _dataContextMsSql.OrderDetailTemps.Update(orderDetailTemp);
        }

        await _dataContextMsSql.SaveChangesAsync();
    }


    public async Task ModifyOrderDetailTempQuantityAsync(
        int id, double quantity)
    {
        var orderDetailTemp =
            await _dataContextMsSql.OrderDetailTemps.FindAsync(id);

        if (orderDetailTemp == null) return;

        orderDetailTemp.Quantity += quantity;

        if (orderDetailTemp.Quantity > 0)
            _dataContextMsSql.OrderDetailTemps.Update(orderDetailTemp);
        else
            _dataContextMsSql.OrderDetailTemps.Remove(orderDetailTemp);

        await _dataContextMsSql.SaveChangesAsync();
    }


    public async Task DeleteDetailTempAsync(int id)
    {
        var orderDetailTemp =
            await _dataContextMsSql.OrderDetailTemps.FindAsync(id);

        if (orderDetailTemp == null) return;

        _dataContextMsSql.OrderDetailTemps.Remove(orderDetailTemp);

        await _dataContextMsSql.SaveChangesAsync();
    }


    public async Task<bool> ConfirmOrderAsync(string userName)
    {
        var user = await _userHelper.GetUserByEmailAsync(userName);

        if (user == null) return false;

        var orderDetailsTemp =
            await _dataContextMsSql.OrderDetailTemps
                .Include(odt => odt.Product)
                .Where(odt => odt.User == user)
                .ToListAsync();

        if (orderDetailsTemp.Count == 0) return false;

        var order = new Order
        {
            OrderDate = DateTime.UtcNow,
            WasDeleted = false,
            User = user,
            Items = orderDetailsTemp.Select(odt => new OrderDetail
                {
                    Price = odt.Product.Price,
                    Product = odt.Product,
                    Quantity = odt.Quantity,
                    WasDeleted = false
                })
                .ToList(),
        };

        // _dataContextMsSql.Orders.Add(order);
        await CreateAsync(order);

        _dataContextMsSql.OrderDetailTemps.RemoveRange(orderDetailsTemp);

        await _dataContextMsSql.SaveChangesAsync();

        return true;
    }


    public async Task<bool> DeliverOrder(DeliveryViewModel model)
    {
        var order = await _dataContextMsSql.Orders.FindAsync(model.Id);

        if (order == null) return false;

        order.DeliveryDate = model.DeliveryDate;

        _dataContextMsSql.Orders.Update(order);

        await _dataContextMsSql.SaveChangesAsync();

        return true;
    }
}