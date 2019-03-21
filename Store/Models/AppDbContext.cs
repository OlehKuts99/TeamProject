using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Store.Models
{
    /// <summary>
    /// Context for database, here is current state of tables in database.
    /// </summary>
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Good> Goods { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Storage> Storages { get; set; }
    }
}
