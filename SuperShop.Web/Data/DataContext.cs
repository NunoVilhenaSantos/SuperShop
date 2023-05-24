using Microsoft.EntityFrameworkCore;

namespace SuperShop.Web.Data
{
    public class DataContext : DbContext
    {

        public DbSet<Entity.Product> Products { get; set; }


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            //DbContextOptions


        }
    }
}
