using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SuperShop.Web.Data.Entity;

namespace SuperShop.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<User> _userManager;
        private readonly Random _random;

        public SeedDb(DataContext dataContext, UserManager<User> userManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _dataContext.Database.EnsureCreatedAsync();

            if (!_dataContext.Products.Any())
            {
                AddProducts("Sapatos", _userManager);
                AddProducts("Ratos", _userManager);
                AddProducts("Ratazanas", _userManager);
                AddProducts("Unhas", _userManager);
            }

            //await CheckCountriesAsync();
            //await CheckCitiesAsync();
            //await CheckCompanyAsync();
            //await CheckUserAsync();
            //await CheckProductsAsync();

            await _dataContext.SaveChangesAsync();
        }

        private void AddProducts(string name, UserManager<User> _userManager)
        {
            _dataContext.Products.Add(
                new Product
                {
                    Name = name, IsAvailable = true,
                    Price = _random.Next(100),
                    Stock = _random.Next(10000),
                    User = _userManager.Users.FirstOrDefault(),
                });
        }
    }
}