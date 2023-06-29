using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuperShop.Web.Data.Entity;

namespace SuperShop.Web.Data.Repositories;

public interface IOrderRepository : IGenericRepository<Order>
{

    Task <IQueryable<Order>> GetOrdersFromUsersAsync(string userName);
}