using System.Linq;
using System.Threading.Tasks;
using SuperShop.Web.Data.Entities;
using SuperShop.Web.Models;

namespace SuperShop.Web.Data.Repositories;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<Order> GetOrdersAsync(int id);


    Task<IQueryable<Order>> GetOrdersAsync(string userName);
    // Task<EnumerableQuery<Order>> GetOrdersAsync(string userName);


    Task<IQueryable<OrderDetailTemp>> GetDetailsTempAsync(string userName);
    // Task<EnumerableQuery<OrderDetailTemp>> GetDetailsTempAsync(string userName);


    Task AddItemToOrderAsync(AddItemViewModel model, string userName);


    Task ModifyOrderDetailTempQuantityAsync(int id, double quantity);


    Task DeleteDetailTempAsync(int id);


    Task<bool> ConfirmOrderAsync(string userName);


    Task<bool> DeliverOrder(DeliveryViewModel model);
}