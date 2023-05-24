using Microsoft.EntityFrameworkCore;
using SuperShop.Web.Data.Entity;

namespace SuperShop.Web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(
            options)
        {
            //DbContextOptions
        }

        public DbSet<Product> Products { get; set; }
    }
}