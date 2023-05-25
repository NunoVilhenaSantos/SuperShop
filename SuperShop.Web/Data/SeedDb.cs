using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuperShop.Web.Data.Entity;

namespace SuperShop.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _dataContext;
        private readonly Random _random;

        public SeedDb(DataContext dataContext)
        {
            _dataContext = dataContext;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _dataContext.Database.EnsureCreatedAsync();

            if (!_dataContext.Products.Any())
            {
                AddProducts("Sapatos");
                AddProducts("Ratos");
                AddProducts("Ratazanas");
                AddProducts("Unhas");
            }

            //await CheckCountriesAsync();
            //await CheckCitiesAsync();
            //await CheckCompanyAsync();
            //await CheckUserAsync();
            //await CheckProductsAsync();

            await _dataContext.SaveChangesAsync();
        }

        private void AddProducts(string name)
        {
            _dataContext.Products.Add(
                new Product
                {
                    Name = name, IsAvailable = true,
                    Price = _random.Next(100),
                    Stock = _random.Next(10000)
                });
        }
    }
}