using BuildSecureNorthwind.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BuildSecureNorthwind.Contexts
{
    public class NorthwindContext : DbContext
    {
        public NorthwindContext(DbContextOptions<NorthwindContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
