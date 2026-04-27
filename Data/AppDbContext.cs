using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Models;

namespace ECommerceAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Clothing" }
            );

            modelBuilder.Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique();

            modelBuilder.Entity<Order>()
                .Property(o => o.PaymentStatus)
                .HasConversion<string>();

            base.OnModelCreating(modelBuilder);
        

        modelBuilder.Entity<Product>().HasData(
                new Product 
                {
                    Id = 1, 
                    Name = "Laptop", 
                    Description = "High performance laptop",  
                    Price = 1000, 
                    Stock = 10, 
                    CategoryId = 1 
                },
                new Product 
                { 
                    Id = 2, 
                    Name = "T-Shirt",
                    Description = "Quality casual wear t-shirt",
                    Price = 20, 
                    Stock = 50, 
                    CategoryId = 2 
                }
            );

            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>();
        }

        // DbSets
        // Product-Related Dbsets
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        // User
        public DbSet<User> Users { get; set; }

        // Cart-Related Dbsets
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        // Order-Related Dbsets
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}