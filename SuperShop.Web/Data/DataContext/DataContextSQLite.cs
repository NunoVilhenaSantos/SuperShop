using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SuperShop.Web.Data.Entities;

namespace SuperShop.Web.Data.DataContext;
// public class DataContextMSSQL : DbContext

public class DataContextSqLite : IdentityDbContext<User>
{
    public DataContextSqLite(DbContextOptions<DataContextSqLite> options) :
        base(options)
    {
    }


    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderDetail> OrderDetails { get; set; }

    public DbSet<OrderDetailTemp> OrderDetailTemps { get; set; }


    public DbSet<City> Cities { get; set; }

    public DbSet<Country> Countries { get; set; }


    public DbSet<Product> Products { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        foreach (
            var relationship in
            builder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.Restrict;

        base.OnModelCreating(builder);
    }
}