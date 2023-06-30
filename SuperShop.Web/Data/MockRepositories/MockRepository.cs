using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuperShop.Web.Data.Entities;
using SuperShop.Web.Data.Repositories;

namespace SuperShop.Web.Data.MockRepositories;

public class MockRepository : IGenericRepository<Product>
{
    public IQueryable<Product> GetAll()
    {
        var products = new List<Product>
        {
            new() {Id = 1, Name = "Product 1", Price = 10},
            new() {Id = 2, Name = "Product 2", Price = 20},
            new() {Id = 3, Name = "Product 3", Price = 30},
            new() {Id = 4, Name = "Product 4", Price = 40},
            new() {Id = 5, Name = "Product 5", Price = 50}
        };

        return products.AsQueryable();
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task CreateAsync(Product entity)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(Product entity)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Product entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ExistAsync(int id)
    {
        throw new NotImplementedException();
    }
}