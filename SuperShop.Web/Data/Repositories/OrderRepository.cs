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
    private readonly DataContextMSSQL _dataContextMssql;
    private readonly IUserHelper _userHelper;


    public OrderRepository(
        IUserHelper userHelper,
        DataContextMSSQL dataContextMssql
    ) : base(dataContextMssql)
    {
        _dataContextMssql = dataContextMssql;
        _userHelper = userHelper;
    }


    public async Task<IQueryable<Order>> GetOrdersAsync(string userName)
    {
        var user = await _userHelper.GetUserByEmailAsync(userName);


        if (user == null) return null;


        if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
            return _dataContextMssql.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .OrderByDescending(o => o.OrderDate);


        return _dataContextMssql.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .Where(o => o.User == user)
            .OrderByDescending(o => o.OrderDate);
    }

    public async Task<IQueryable<OrderDetailTemp>> GetDetailsTempAsync(
        string userName)
    {
        var user = await _userHelper.GetUserByEmailAsync(userName);


        if (user == null) return null;


        if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
            return _dataContextMssql.OrderDetailTemps
                .Include(o => o.User)
                .Include(p => p.Product)
                .OrderByDescending(o => o.Product.Name);


        return _dataContextMssql.OrderDetailTemps
            .Include(o => o.Product)
            .Where(o => o.User == user)
            .OrderByDescending(o => o.Product.Name);
    }


    public async Task AddItemToOrderAsync(
        AddItemViewModel model, string userName)
    {
        throw new NotImplementedException();
    }


    public async Task ModifyOrderDetailTempQuantityAsync(
        int id, double quantity)
    {
        throw new NotImplementedException();
    }


    public async Task DeleteDetailTempAsync(int id)
    {
        throw new NotImplementedException();
    }


    public async Task<bool> ConfirmOrderAsync(string userName)
    {
        throw new NotImplementedException();
    }
}