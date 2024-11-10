using Microsoft.EntityFrameworkCore;
using ST10150702_CLDV6212_POE.Models;
namespace ST10150702_CLDV6212_POE.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        // DbSet for Customers
        public DbSet<Customer> Customers { get; set; }

        // DbSet for Products
        public DbSet<Product> Products { get; set; }

        // DbSet for Orders
        public DbSet<Order> Orders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasKey(c => c.cID); // Define primary key for Customer

            modelBuilder.Entity<Product>()
                .HasKey(p => p.pID); // Define primary key for Product

            modelBuilder.Entity<Order>()
                .HasKey(o => o.oID); // Define primary key for Order

            base.OnModelCreating(modelBuilder);
        }

    }
}
