using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SuperShop.Web.Data.Entity;

namespace SuperShop.Web.Data.DataContext;
// public class DataContextMSSQL : DbContext

public class DataContextMySQL : IdentityDbContext<User>
{
    public DataContextMySQL(DbContextOptions<DataContextMySQL> options) :
        base(options)
    {
    }


    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderDetail> OrderDetails { get; set; }

    public DbSet<OrderDetailTemp> OrderDetailTemps { get; set; }

    public DbSet<Product> Products { get; set; }
}