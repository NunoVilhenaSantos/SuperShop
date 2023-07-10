using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SuperShop.Web.Data.DataContext;
using SuperShop.Web.Data.Entities;
using SuperShop.Web.Helpers;

namespace SuperShop.Web.Data;

public class SeedDb
{
    // private readonly UserManager<User> _userManager;


    private readonly DataContextMsSql _dataContextMsSql;
    private readonly DataContextMySql _dataContextMySql;
    private readonly DataContextSqLite _dataContextSqLite;
    private readonly Random _random;

    private readonly IUserHelper _userHelper;


    // public SeedDb(DataContextMSSQL dataContextMssql, UserManager<User> userManager)
    public SeedDb(
        DataContextMsSql dataContextMsSql,
        DataContextMySql dataContextMySql,
        DataContextSqLite dataContextSqLite,
        IUserHelper userHelper)
    {
        _random = new Random();
        _userHelper = userHelper;

        _dataContextMsSql = dataContextMsSql;
        _dataContextMySql = dataContextMySql;
        _dataContextSqLite = dataContextSqLite;
        // _userManager = userManager;
    }


    public async Task SeedAsync()
    {
        // await _dataContextMssql.Database.EnsureCreatedAsync();
        // await _dataContextMssql.Database.EnsureCreatedAsync();
        // await _dataContextSqLite.Database.EnsureCreatedAsync();


        await _dataContextMsSql.Database.MigrateAsync();
        await _dataContextMySql.Database.MigrateAsync();
        await _dataContextSqLite.Database.MigrateAsync();


        // adiciona roles ao sistema
        await _userHelper.CheckRoleAsync("Admin");
        await _userHelper.CheckRoleAsync("Customer");


        AddCitiesAndCountries();
        await AddUsers();

        //var user =
        //    await _userManager.FindByEmailAsync(
        //        "nunovilhenasantos@msn.com");

        var user =
            await _userHelper.GetUserByEmailAsync(
                "nunovilhenasantos@msn.com");


        if (!_dataContextMsSql.Products.Any())
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

        await _dataContextMsSql.SaveChangesAsync();
    }

    private async Task AddUsers()
    {
        // ----------------------------------------------------------------- //
        // criar users admin
        // ----------------------------------------------------------------- //

        //
        // nunovilhenasantos@msn.com
        //
        var user = new User
        {
            FirstName = "Nuno",
            LastName = "Santos",
            Email = "nunovilhenasantos@msn.com",
            UserName = "nunovilhenasantos@msn.com",
            PhoneNumber = "211333555",
            Address = "Rua do Ouro, 123",
            CountryId = 1,
            City = await _dataContextMsSql.Cities.FindAsync(1),
        };

        // var result = await _userManager.CreateAsync(user, "Passw0rd");
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

        // adiciona role ao user
        await _userHelper.AddUserToRoleAsync(user, "Admin");


        // verifica se o user está na role
        var isEnrolled =
            await _userHelper.IsUserInRoleAsync(user, "Admin");

        // tenta registar o user na role
        if (!isEnrolled)
            await _userHelper.AddUserToRoleAsync(user, "Admin");


        // ----------------------------------------------------------------- //
        //
        // nuno.santos.26288@formandos.cinel.pt
        //
        user = new User
        {
            FirstName = "Nuno",
            LastName = "Santos",
            Email = "nuno.santos.26288@formandos.cinel.pt",
            UserName = "nuno.santos.26288@formandos.cinel.pt",
            PhoneNumber = "211333555",
            Address = "Rua do Ouro, 123",
            CountryId = 1,
            City = await _dataContextMsSql.Cities.FindAsync(2),
        };

        // var result = await _userManager.CreateAsync(user, "Passw0rd");
        result = await _userHelper
            .AddUserAsync(user, "Passw0rd");


        if (result != IdentityResult.Success)
        {
            result.Errors.ToList()
                .ForEach(
                    e => Console.WriteLine(e.Description));

            throw new InvalidOperationException(
                "Could not create the user in Seeder");
        }


        // adiciona role ao user
        await _userHelper.AddUserToRoleAsync(user, "Admin");


        // verifica se o user está na role
        isEnrolled = await _userHelper.IsUserInRoleAsync(user, "Admin");


        // tenta registar o user na role
        if (!isEnrolled)
            await _userHelper.AddUserToRoleAsync(user, "Admin");


        // ----------------------------------------------------------------- //
        // criar users customers
        // ----------------------------------------------------------------- //

        //
        // Carmelita.Jones@yopmail.pt
        //
        user = new User
        {
            FirstName = "Carmelita",
            LastName = "Jones",
            Email = "Carmelita.Jones@yopmail.pt",
            UserName = "Carmelita.Jones@yopmail.pt",
            PhoneNumber = "211333555",
            Address = "Rua do Ouro, 123",
            CountryId = 1,
            City = await _dataContextMsSql.Cities.FindAsync(3),
        };

        // var result = await _userManager.CreateAsync(user, "Passw0rd");
        result = await _userHelper
            .AddUserAsync(user, "Passw0rd");


        if (result != IdentityResult.Success)
        {
            result.Errors.ToList()
                .ForEach(
                    e => Console.WriteLine(e.Description));

            throw new InvalidOperationException(
                "Could not create the user in Seeder");
        }


        // adiciona role ao user
        await _userHelper.AddUserToRoleAsync(user, "Customer");


        // verifica se o user está na role
        isEnrolled =
            await _userHelper.IsUserInRoleAsync(user, "Customer");


        // tenta registar o user na role
        if (!isEnrolled)
            await _userHelper.AddUserToRoleAsync(user, "Customer");


        // ----------------------------------------------------------------- //
        //
        // cria user customer
        //
        user = new User
        {
            FirstName = "Rafael",
            LastName = "Jones",
            Email = "Rafael.Jones@yopmail.pt",
            UserName = "Rafael.Jones@yopmail.pt",
            PhoneNumber = "211333555",
            Address = "Rua do Ouro, 123",
            CountryId = 1,
            City = await _dataContextMsSql.Cities.FindAsync(3),
        };

        // var result = await _userManager.CreateAsync(user, "Passw0rd");
        result = await _userHelper
            .AddUserAsync(user, "Passw0rd");


        if (result != IdentityResult.Success)
        {
            result.Errors.ToList()
                .ForEach(
                    e => Console.WriteLine(e.Description));

            throw new InvalidOperationException(
                "Could not create the user in Seeder");
        }


        // adiciona role ao user
        await _userHelper.AddUserToRoleAsync(user, "Customer");


        // verifica se o user está na role
        isEnrolled =
            await _userHelper.IsUserInRoleAsync(user, "Customer");


        // tenta registar o user na role
        if (!isEnrolled)
            await _userHelper.AddUserToRoleAsync(user, "Customer");
    }


    private void AddCitiesAndCountries()
    {
        var cities = new List<City>();
        cities.Add(new City {Name = "Lisboa"});
        cities.Add(new City {Name = "Porto"});
        cities.Add(new City {Name = "Coimbra"});


        _dataContextMsSql.Countries.Add(
            new Country {Name = "Portugal", Cities = cities});

        cities = new List<City>();
        cities.Add(new City {Name = "Madrid"});
        cities.Add(new City {Name = "Salamanca"});
        cities.Add(new City {Name = "Sevilha"});


        _dataContextMsSql.Countries.Add(
            new Country {Name = "Espanha", Cities = cities});
    }

    private void AddProducts(string name, User user)
    {
        var currentDate = DateTime.Now;

        var random = new Random();

        // Generate random TimeSpan for
        // LastPurchase and LastSale within a range of 30 days
        var purchaseTimeSpan = TimeSpan.FromDays(random.Next(30));
        var saleTimeSpan = TimeSpan.FromDays(random.Next(30));

        var randomPurchaseDate = currentDate.Subtract(purchaseTimeSpan);
        var randomSaleDate = currentDate.Subtract(saleTimeSpan);

        _dataContextMsSql.Products.Add(
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