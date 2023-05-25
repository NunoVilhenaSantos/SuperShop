using SuperShop.Web.Data.Entity;

namespace SuperShop.Web.Data
{
    public class ProductRepository : GenericRepository<Product>,
        IProductsRepository
    {
        public ProductRepository(DataContext dataContext) : base(dataContext)
        {
        }
    }
}