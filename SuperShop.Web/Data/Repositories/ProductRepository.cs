using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuperShop.Web.Data.DataContext;
using SuperShop.Web.Data.Entities;

namespace SuperShop.Web.Data.Repositories;

public class ProductRepository :
    GenericRepository<Product>, IProductsRepository
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


    public IEnumerable<SelectListItem> GetComboProducts()
    {
        var list =
            _dataContextMssql.Products
                .Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }).ToList();

        list.Insert(0, new SelectListItem
        {
            Text = "(Select a product...)",
            Value = "0"
        });

        return list;
    }
}