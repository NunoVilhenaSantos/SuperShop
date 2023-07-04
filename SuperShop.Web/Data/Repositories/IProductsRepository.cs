using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using SuperShop.Web.Data.Entities;

namespace SuperShop.Web.Data.Repositories;

public interface IProductsRepository : IGenericRepository<Product>
{
    public IQueryable GetAllWithUsers();

    // public IQueryable GetAllWithUsersAndOrders();
    //
    // public IQueryable GetAllWithUsersAndOrders(int id);
    //
    // public IQueryable GetAllWithUsersAndOrders(string userName);
    //
    // public IQueryable GetAllWithUsersAndOrdersAndDetails(int id);
    //
    // public IQueryable GetAllWithUsersAndOrdersAndDetails(string userName);
    //
    // public IQueryable GetAllWithUsersAndOrdersAndDetailsTemp(int id);
    //
    // public IQueryable GetAllWithUsersAndOrdersAndDetailsTemp(string userName);

    public IEnumerable<SelectListItem> GetComboProducts();
}