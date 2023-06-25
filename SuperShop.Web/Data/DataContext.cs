using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SuperShop.Web.Data.Entity;

namespace SuperShop.Web.Data;
// public class DataContext : DbContext

public class DataContext : IdentityDbContext<User>
{
    public DataContext(DbContextOptions<DataContext> options) :
        base(options)
    {
        
    }


    public DbSet<Product> Products { get; set; }
}