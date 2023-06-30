using System.Linq;
using System.Threading.Tasks;
using SuperShop.Web.Data.Entities;

namespace SuperShop.Web.Data.Repositories;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<IQueryable<Order>> GetOrdersAsync(string userName);


    Task<IQueryable<OrderDetailTemp>> GetDetailsTempAsync(string userName);
}