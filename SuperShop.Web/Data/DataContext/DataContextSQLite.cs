using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SuperShop.Web.Data.Entity;

namespace SuperShop.Web.Data.DataContext;
// public class DataContextMSSQL : DbContext

public class DataContextSQLite : IdentityDbContext<User>
{
    public DataContextSQLite(DbContextOptions<DataContextSQLite> options) :
        base(options)
    {
    }


    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderDetail> OrderDetails { get; set; }

    public DbSet<OrderDetailTemp> OrderDetailTemps { get; set; }

    public DbSet<Product> Products { get; set; }


    protected override void OnModelCreating(ModelBuilder modelbuilder)
    {
        foreach (
            var relationship in
            modelbuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.Restrict;

        base.OnModelCreating(modelbuilder);
    }
}