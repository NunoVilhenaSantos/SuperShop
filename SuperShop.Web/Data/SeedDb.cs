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
    private readonly DataContextMsSql _dataContextMsSql;
    private readonly DataContextMySql _dataContextMySql;
    private readonly DataContextSqLite _dataContextSqLite;
    private readonly Random _random;

    private readonly IUserHelper _userHelper;

    private readonly UserManager<User> _userManager;


    // public SeedDb(DataContextMSSQL dataContextMsSql, UserManager<User> userManager)
    public SeedDb(
        IUserHelper userHelper,
        DataContextMsSql dataContextMsSql,
        DataContextMySql dataContextMySql,
        DataContextSqLite dataContextSqLite
    )
    {
        _random = new Random();
        _userHelper = userHelper;

        // _userManager = userManager;

        _dataContextMsSql = dataContextMsSql;
        _dataContextMySql = dataContextMySql;
        _dataContextSqLite = dataContextSqLite;
    }


    public async Task SeedAsync()
    {
        // await _dataContextMsSql.Database.EnsureCreatedAsync();
        // await _dataContextMySql.Database.EnsureCreatedAsync();
        // await _dataContextSqLite.Database.EnsureCreatedAsync();


        await _dataContextMsSql.Database.MigrateAsync();
        await _dataContextMySql.Database.MigrateAsync();
        await _dataContextSqLite.Database.MigrateAsync();


        // adiciona roles ao sistema
        await _userHelper.CheckRoleAsync("Admin");
        await _userHelper.CheckRoleAsync("Employee");
        await _userHelper.CheckRoleAsync("Customer");
        await _userHelper.CheckRoleAsync("Manager");
        await _userHelper.CheckRoleAsync("Salesman");
        await _userHelper.CheckRoleAsync("Warehouse");
        await _dataContextMsSql.SaveChangesAsync();


        AddCitiesAndCountries();
        await AddUsers();


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

        Console.WriteLine("debug zone");

        var userEmail = "nunovilhenasantos@msn.com";

        var user =
            await _userHelper.GetUserByEmailAsync(userEmail);

        if (user == null)
        {
            Console.WriteLine("user not found");

            user = new User
            {
                FirstName = "Nuno",
                LastName = "Santos",
                Email = userEmail,
                UserName = userEmail,
                PhoneNumber = "211333555",
                Address = "Rua do Ouro, 123",
                CountryId = 1,
                City = await _dataContextMsSql.Cities.FindAsync(1)
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
        }

        // ----------------------------------------------------------------- //
        //
        // nuno.santos.26288@formandos.cinel.pt
        //

        userEmail = "nuno.santos.26288@formandos.cinel.pt";

        user =
            await _userHelper.GetUserByEmailAsync(
                userEmail);

        if (user == null)
        {
            Console.WriteLine("user not found");
            // return;

            user = new User
            {
                FirstName = "Nuno",
                LastName = "Santos",
                Email = userEmail,
                UserName = userEmail,
                PhoneNumber = "211333555",
                Address = "Rua do Ouro, 123",
                CountryId = 1,
                City = await _dataContextMsSql.Cities.FindAsync(2)
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
        }

        // ----------------------------------------------------------------- //
        // criar users customers
        // ----------------------------------------------------------------- //

        //
        // Carmelita.Jones@yopmail.pt
        //

        userEmail = "Carmelita.Jones@yopmail.pt";

        user =
            await _userHelper.GetUserByEmailAsync(
                userEmail);

        if (user == null)
        {
            Console.WriteLine("user not found");
            // return;


            user = new User
            {
                FirstName = "Carmelita",
                LastName = "Jones",
                Email = userEmail,
                UserName = userEmail,
                PhoneNumber = "211333555",
                Address = "Rua do Ouro, 123",
                CountryId = 1,
                City = await _dataContextMsSql.Cities.FindAsync(3)
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
            await _userHelper.AddUserToRoleAsync(user, "Customer");


            // verifica se o user está na role
            var isEnrolled =
                await _userHelper.IsUserInRoleAsync(user, "Customer");


            // tenta registar o user na role
            if (!isEnrolled)
                await _userHelper.AddUserToRoleAsync(user, "Customer");
        }


        // ----------------------------------------------------------------- //
        //
        // cria user customer
        //

        userEmail = "Rafael.Jones@yopmail.pt";

        user =
            await _userHelper.GetUserByEmailAsync(
                userEmail);

        if (user == null)
        {
            Console.WriteLine("user not found");
            // return;


            user = new User
            {
                FirstName = "Rafael",
                LastName = "Jones",
                Email = userEmail,
                UserName = userEmail,
                PhoneNumber = "211333555",
                Address = "Rua do Ouro, 123",
                CountryId = 1,
                City = await _dataContextMsSql.Cities.FindAsync(3)
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
            await _userHelper.AddUserToRoleAsync(user, "Customer");


            // verifica se o user está na role
            var isEnrolled =
                await _userHelper.IsUserInRoleAsync(user, "Customer");


            // tenta registar o user na role
            if (!isEnrolled)
                await _userHelper.AddUserToRoleAsync(user, "Customer");
        }
    }


    private void AddCitiesAndCountries()
    {
        if (_dataContextMsSql.Countries.Any()) return;


        var cities = new List<City>();
        cities.Add(new City
        {
            Name = "Luanda", WasDeleted = false
        });
        cities.Add(new City
        {
            Name = "Lobito", WasDeleted = false
        });
        cities.Add(new City
        {
            Name = "Benguela", WasDeleted = false
        });

        _dataContextMsSql.Countries.Add(
            new Country
            {
                Name = "Angola", Cities = cities, WasDeleted = false
            });


        cities = new List<City>();
        cities.Add(new City
        {
            Name = "Lisboa", WasDeleted = false
        });
        cities.Add(new City
        {
            Name = "Porto", WasDeleted = false
        });
        cities.Add(new City
        {
            Name = "Coimbra", WasDeleted = false
        });


        _dataContextMsSql.Countries.Add(
            new Country
            {
                Name = "Portugal", Cities = cities, WasDeleted = false
            });

        cities = new List<City>();
        cities.Add(new City
        {
            Name = "Madrid", WasDeleted = false
        });
        cities.Add(new City
        {
            Name = "Salamanca", WasDeleted = false
        });
        cities.Add(new City
        {
            Name = "Sevilha", WasDeleted = false
        });


        _dataContextMsSql.Countries.Add(
            new Country
            {
                Name = "Espanha", Cities = cities, WasDeleted = false
            });


        _dataContextMsSql.SaveChanges();
    }
	
	

    private void AddProducts(string name, User user)
    {
        var currentDate = DateTime.Now;

        // Generate random TimeSpan for
        // LastPurchase and LastSale within a range of 30 days
        var purchaseTimeSpan = TimeSpan.FromDays(_random.Next(30));
        var saleTimeSpan = TimeSpan.FromDays(_random.Next(30));

        var randomPurchaseDate = currentDate.Subtract(purchaseTimeSpan);
        var randomSaleDate = currentDate.Subtract(saleTimeSpan);

        _dataContextMsSql.Products.Add(
            new Product
            {
                // Id = id,
                Name = name,
                Price = _random.Next(100),

                ImageUrl = string.Empty,
                ImageId = Guid.Empty,

                LastPurchase = randomPurchaseDate,
                LastSale = randomSaleDate,
                IsAvailable = true,
                Stock = _random.Next(10000),
                User = user,
                WasDeleted = false
            });


        _dataContextMsSql.SaveChanges();
    }
}