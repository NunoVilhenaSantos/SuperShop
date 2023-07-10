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
    private readonly DataContextMsSql _dataContextMsSql;

    public ProductRepository(
        DataContextMsSql dataContextMsSql,
        DataContextMySql dataContextMySql,
        DataContextSqLite dataContextSqLite
    ) : base(dataContextMsSql, dataContextMySql, dataContextSqLite)
    {
        _dataContextMsSql = dataContextMsSql;
    }


    public IQueryable GetAllWithUsers()
    {
        return _dataContextMsSql
            .Products
            .Include(p => p.User);
    }


    public IEnumerable<SelectListItem> GetComboProducts()
    {
        var list =
            _dataContextMsSql.Products
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