﻿using System.Linq;
using Microsoft.EntityFrameworkCore;
using SuperShop.Web.Data.DataContext;
using SuperShop.Web.Data.Entities;

namespace SuperShop.Web.Data.Repositories;

public class ProductRepository : GenericRepository<Product>,
    IProductsRepository
{
    private readonly DataContextMSSQL _dataContextMssql;

    public ProductRepository(DataContextMSSQL dataContextMssql) : base(
        dataContextMssql)
    {
        _dataContextMssql = dataContextMssql;
    }


    public IQueryable GetAllWithUsers()
    {
        return _dataContextMssql
            .Products
            .Include(
                p => p.User);
    }
}