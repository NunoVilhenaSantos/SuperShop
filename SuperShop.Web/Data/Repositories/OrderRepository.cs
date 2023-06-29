using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuperShop.Web.Data.DataContext;
using SuperShop.Web.Data.Entity;
using SuperShop.Web.Helpers;

namespace SuperShop.Web.Data.Repositories;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    private readonly IUserHelper _userHelper;
    private readonly DataContextMSSQL _dataContextMssql;


    public OrderRepository(
        IUserHelper userHelper,
        DataContextMSSQL dataContextMssql
    ) : base(dataContextMssql)
    {
        _dataContextMssql = dataContextMssql;
        _userHelper = userHelper;
    }


    public async Task<IQueryable<Order>> GetOrdersFromUsersAsync(
        string userName)
    {
        var user = await _userHelper.GetUserByEmailAsync(userName);

        if (user == null) return null;

        if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
        {
            return _dataContextMssql.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .OrderByDescending(o => o.OrderDate);
        }

        // return _dataContextMssql.Orders
        //     .Include(o => o.Items)
        //     .ThenInclude(i => i.Product)
        //     .Where(o => o.User.Id == user.Id)
        //     .OrderByDescending(o => o.OrderDate);

        // return GetAll().Where(o => o.User.Id == user.Id)
        //     .OrderByDescending(o => o.OrderDate);

        return await Task.Run(() =>
            GetAll().Where(o => o.User.Id == user.Id)
                .OrderByDescending(o => o.OrderDate));
    }
}