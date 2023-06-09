using System.Linq;
using Microsoft.EntityFrameworkCore;
using SuperShop.Web.Data.Entity;

namespace SuperShop.Web.Data;

public class ProductRepository : GenericRepository<Product>,
    IProductsRepository
{
    private readonly DataContext _dataContext;

    public ProductRepository(DataContext dataContext) : base(dataContext)
    {
        _dataContext = dataContext;
    }


    public IQueryable GetAllWithUsers()
    {
        return _dataContext
            .Products
            .Include(
                p => p.User);
    }
}