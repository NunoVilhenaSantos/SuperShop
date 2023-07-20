using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuperShop.Web.Data.DataContext;
using SuperShop.Web.Data.Entities;
using SuperShop.Web.Data.Repositories;
using SuperShop.Web.Helpers;

namespace SuperShop.Web.Data.MockRepositories;

public class MockRepository : IGenericRepository<Product>
{
    private readonly DataContextMsSql _dataContextMsSql;
    private readonly DataContextMySql _dataContextMySql;
    private readonly DataContextSqLite _dataContextSqLite;

    private readonly Random _random;

    private readonly IUserHelper _userHelper;

    public MockRepository(
        Random random, IUserHelper userHelper,
        DataContextMySql dataContextMySql, DataContextMsSql dataContextMsSql,
        DataContextSqLite dataContextSqLite)
    {
        _random = random;
        _userHelper = userHelper;
        _dataContextMySql = dataContextMySql;
        _dataContextMsSql = dataContextMsSql;
        _dataContextSqLite = dataContextSqLite;
    }


    public IQueryable<Product> GetAll()
    {
        var user = _userHelper
            .GetUserByEmailAsync("nuno.santos.26288@formandos.cinel.pt").Result;

        var country = _dataContextMsSql.Countries.FirstOrDefault();

        var city = _dataContextMsSql.Cities
            .Include(c => c)
            .ThenInclude(c => country).FirstOrDefault();


        // user = new User
        // {
        //     Id = "1",
        //     FirstName = "John",
        //     LastName = "Doe",
        //     Email = "",
        //     UserName = "",
        //     PhoneNumber = "",
        //     Address = "",
        //     City = city,
        //     CountryId = country.Id,
        // };


        var products = new List<Product>
        {
            new()
            {
                Id = 1, Name = "Product 1", Price = _random.Next(0, 99),
                IsAvailable = true, Stock = _random.Next(0, 9999),
                User = user, WasDeleted = false
            },
            new()
            {
                Id = 2, Name = "Product 2", Price = _random.Next(0, 99),
                IsAvailable = true, Stock = _random.Next(0, 9999),
                User = user, WasDeleted = false
            },
            new()
            {
                Id = 3, Name = "Product 3", Price = _random.Next(0, 99),
                IsAvailable = true, Stock = _random.Next(0, 9999),
                User = user, WasDeleted = false
            },
            new()
            {
                Id = 4, Name = "Product 4", Price = _random.Next(0, 99),
                IsAvailable = true, Stock = _random.Next(0, 9999),
                User = user, WasDeleted = false
            },
            new()
            {
                Id = 5, Name = "Product 5", Price = _random.Next(0, 99),
                IsAvailable = true, Stock = _random.Next(0, 9999),
                User = user, WasDeleted = false
            }
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