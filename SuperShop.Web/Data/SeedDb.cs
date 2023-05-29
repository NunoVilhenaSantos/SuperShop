using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SuperShop.Web.Data.Entity;
using SuperShop.Web.Helpers;

namespace SuperShop.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _dataContext;
        private readonly Random _random;
        private readonly IUserHelper _userHelper;

        // private readonly UserManager<User> _userManager;


        //public SeedDb(DataContext dataContext, UserManager<User> userManager)
        public SeedDb(DataContext dataContext, IUserHelper userHelper)
        {
            _random = new Random();
            _userHelper = userHelper;
            _dataContext = dataContext;
            // _userManager = userManager;
        }


        public async Task SeedAsync()
        {
            await _dataContext.Database.EnsureCreatedAsync();

            //var user =
            //    await _userManager.FindByEmailAsync(
            //        "nunovilhenasantos@msn.com");

            var user =
                await _userHelper.GetUserByEmailAsync(
                    "nunovilhenasantos@msn.com");

            if (user == null)
            {
                user = new User
                {
                    FirstName = "Nuno",
                    LastName = "Santos",
                    Email = "nunovilhenasantos@msn.com",
                    UserName = "nunovilhenasantos",
                    PhoneNumber = "211333555"
                };

                //var result = await _userManager.CreateAsync(user, "Passw0rd");
                var result = await _userHelper.AddUserAsync(user, "Passw0rd");

                if (result != IdentityResult.Success)
                {
                    result.Errors.ToList()
                        .ForEach(
                            e => Console.WriteLine(e.Description));

                    throw new InvalidOperationException(
                        "Could not create the user in Seeder");

                    return;
                }
            }

            if (!_dataContext.Products.Any())
            {
                AddProducts("Ténis", user);
                AddProducts("T-Shirt", user);
                AddProducts("Sapatos", user);
                AddProducts("Camisola", user);
                AddProducts("Unhas Gel", user);
                AddProducts("Rato Y800", user);
                AddProducts("Teclado X300", user);
                AddProducts("Teclado / Rato", user);
                AddProducts("Webcam OPT 3500", user);
                AddProducts("Microfone W6500", user);
            }

            // await CheckCountriesAsync();
            // await CheckCitiesAsync();
            // await CheckCompanyAsync();
            // await CheckUserAsync();
            // await CheckProductsAsync();

            await _dataContext.SaveChangesAsync();
        }


        private void AddProducts(string name, User user)
        {
            _dataContext.Products.Add(
                new Product
                {
                    Name = name, IsAvailable = true,
                    Price = _random.Next(100),
                    Stock = _random.Next(10000),
                    User = user
                });
        }
    }
}