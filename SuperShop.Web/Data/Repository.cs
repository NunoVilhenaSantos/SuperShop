using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuperShop.Web.Data.Entity;

namespace SuperShop.Web.Data
{
    public class Repository : IRepository
    {
        private readonly DataContext _dataContext;


        public Repository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        /// <summary>
        ///     obtém uma lista de produtos
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Product> GetProducts()
        {
            return _dataContext.Products.OrderBy(p => p.Name);
        }

        /// <summary>
        ///     obtém um produto pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product GetProduct(int id)
        {
            return _dataContext.Products.Find(id);
        }


        /// <summary>
        ///     adiciona um produto na lista via id
        /// </summary>
        /// <param name="id"></param>
        public void AddProduct(int id)
        {
            var product = GetProduct(id);
            if (product != null) _dataContext.Products.Add(product);
        }


        /// <summary>
        ///     adiciona um produto na lista via product
        /// </summary>
        /// <param name="product"></param>
        public void AddProduct(Product product)
        {
            if (product != null)
                _dataContext.Products.Add(product);
        }

        /// <summary>
        ///     atualiza um produto na lista via id
        /// </summary>
        /// <param name="id"></param>
        public void UpdateProduct(int id)
        {
            var product = GetProduct(id);
            if (product != null) _dataContext.Products.Update(product);
        }

        /// <summary>
        ///     atualiza um produto na lista via product
        /// </summary>
        /// <param name="product"></param>
        public void UpdateProduct(Product product)
        {
            if (product != null) _dataContext.Products.Update(product);
        }


        /// <summary>
        ///     apaga o produto da lista via id
        /// </summary>
        /// <param name="id"></param>
        public void DeleteProduct(int id)
        {
            var product = GetProduct(id);

            if (product != null) _dataContext.Products.Remove(product);
        }


        /// <summary>
        ///     apaga o produto da lista via product
        /// </summary>
        /// <param name="id"></param>
        public void DeleteProduct(Product product)
        {
            if (product != null) _dataContext.Products.Remove(product);
        }


        /// <summary>
        ///     verifica se existe um produto na lista via id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ProductExists(int id)
        {
            return _dataContext.Products.Any(p => p.Id == id);
        }


        /// <summary>
        ///     guarda tudo se houver alterações para guardar
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}