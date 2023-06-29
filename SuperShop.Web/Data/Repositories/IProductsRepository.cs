using System.Linq;
using SuperShop.Web.Data.Entity;

namespace SuperShop.Web.Data.Repositories;

public interface IProductsRepository : IGenericRepository<Product>
{
    public IQueryable GetAllWithUsers();
}