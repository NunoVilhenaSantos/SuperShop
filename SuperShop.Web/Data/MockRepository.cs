using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SuperShop.Web.Data.Entity;

namespace SuperShop.Web.Data
{
    public class MockRepository : IRepository
    {
        void IRepository.AddProduct(int id)
        {
            throw new NotImplementedException();
        }

        void IRepository.AddProduct(Product product)
        {
            throw new NotImplementedException();
        }

        void IRepository.DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }

        void IRepository.DeleteProduct(Product product)
        {
            throw new NotImplementedException();
        }

        Product IRepository.GetProduct(int id)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Product> IRepository.GetProducts()
        {
            var products = new List<Product>
            {
                new() {Id = 1, Name = "Product 1", Price = 10},
                new() {Id = 2, Name = "Product 2", Price = 20},
                new() {Id = 3, Name = "Product 3", Price = 30},
                new() {Id = 4, Name = "Product 4", Price = 40},
                new() {Id = 5, Name = "Product 5", Price = 50}
            };

            return products;
        }

        bool IRepository.ProductExists(int id)
        {
            throw new NotImplementedException();
        }

        Task<bool> IRepository.SaveAllAsync()
        {
            throw new NotImplementedException();
        }

        void IRepository.UpdateProduct(int id)
        {
            throw new NotImplementedException();
        }

        void IRepository.UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }
    }
}