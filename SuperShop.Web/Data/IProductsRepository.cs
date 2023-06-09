using System.Linq;
using SuperShop.Web.Data.Entity;

namespace SuperShop.Web.Data;

public interface IProductsRepository : IGenericRepository<Product>
{
    public IQueryable GetAllWithUsers();
}