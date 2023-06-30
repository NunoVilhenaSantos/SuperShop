using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuperShop.Web.Data.DataContext;
using SuperShop.Web.Data.Entities;
using SuperShop.Web.Helpers;

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
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .OrderByDescending(o => o.OrderDate);

        return _dataContextMssql.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .Where(o => o.User.Id == user.Id)
            .OrderByDescending(o => o.OrderDate);
    }

    public async Task<IQueryable<OrderDetailTemp>> GetDetailsTempAsync(
        string userName)
    {
        var user = await _userHelper.GetUserByEmailAsync(userName);

        if (user == null) return null;

        if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
            return _dataContextMssql.OrderDetailTemps
                .Include(p => p.Product)
                .OrderByDescending(o => o.Product.Name);

        return _dataContextMssql.OrderDetailTemps
            .Include(o => o.Product)
            .Where(o => o.User.Id == user.Id)
            .OrderByDescending(o => o.Product.Name);
    }
}