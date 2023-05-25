using SuperShop.Web.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperShop.Web.Data
{
    public interface IRepository
    {
        void AddProduct(int id);
        void AddProduct(Product product);


        void DeleteProduct(int id);
        void DeleteProduct(Product product);


        Product GetProduct(int id);
        IEnumerable<Product> GetProducts();

        bool ProductExists(int id);

        Task<bool> SaveAllAsync();

        void UpdateProduct(int id);
        void UpdateProduct(Product product);
    }
}