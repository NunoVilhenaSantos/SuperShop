using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SuperShop.Web.Data.Entity;
using SuperShop.Web.Helpers;

namespace SuperShop.Web.Data;

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
            var result = await _userHelper
                .AddUserAsync(user, "Passw0rd");

            if (result != IdentityResult.Success)
            {
                result.Errors.ToList()
                    .ForEach(
                        e => Console.WriteLine(e.Description));

                throw new InvalidOperationException(
                    "Could not create the user in Seeder");
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
        var currentDate = DateTime.Now;

        var random = new Random();

        // Generate random TimeSpan for LastPurchase and LastSale within a range of 30 days
        var purchaseTimeSpan = TimeSpan.FromDays(random.Next(30));
        var saleTimeSpan = TimeSpan.FromDays(random.Next(30));

        var randomPurchaseDate = currentDate.Subtract(purchaseTimeSpan);
        var randomSaleDate = currentDate.Subtract(saleTimeSpan);

        _dataContext.Products.Add(
            new Product
            {
                // Id = id,
                Name = name,
                Price = _random.Next(100),

                ImageUrl = string.Empty,
                // Property or indexer 'property' cannot be assigned to -- it is read only
                // ImageFullUrl = string.Empty,
                ImageId = Guid.Empty,
                // Property or indexer 'property' cannot be assigned to -- it is read only
                // ImageFullIdUrl = string.Empty,
                ImageIdGcp = Guid.Empty,
                // Property or indexer 'property' cannot be assigned to -- it is read only
                // ImageFullIdGcpUrl = string.Empty,
                ImageIdAws = Guid.Empty,
                // Property or indexer 'property' cannot be assigned to -- it is read only
                // ImageFullIdAwsUrl = string.Empty,

                LastPurchase = randomPurchaseDate,
                LastSale = randomSaleDate,
                IsAvailable = true,
                Stock = _random.Next(10000),
                User = user
            });
    }
}