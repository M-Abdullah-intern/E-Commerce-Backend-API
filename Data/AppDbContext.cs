using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Models;

namespace ECommerceAPI.Data
{
    public class AppDbContext : DbContext
    {
        // Constructor
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Configure the model
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure decimal precision for Product price
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            // Configure one-to-many relationship between Product and ProductImage
            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductImages)
                .WithOne(pi => pi.Product)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
                        
            // Ensure category names are unique
            modelBuilder.Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique();

            // Configure PaymentStatus enum to be stored as string
            modelBuilder.Entity<Order>()
                .Property(o => o.PaymentStatus)
                .HasConversion<string>();

            // Call the base method to ensure any additional configuration is applied
            base.OnModelCreating(modelBuilder);

            // Configure OrderStatus enum to be stored as string
            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>();

            // Configure decimal precision for Order TotalAmount and OrderItem Price
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasPrecision(18, 2);

            // Seed initial categories
            modelBuilder.Entity<Category>()
                .HasData(new Category
                {
                    CategoryId = 1,
                    Name = "Electronics"
                },
                new Category
                {
                    CategoryId = 2,
                    Name = "Clothing"
                },
                new Category
                {
                    CategoryId = 3,
                    Name = "Books"
                },
                new Category
                {
                    CategoryId = 4,
                    Name = "Home Appliances"
                }
            );
        }

        // DbSets
        // Product System Dbsets
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        // Auth System Dbsets
        public DbSet<User> Users { get; set; }

        // Cart System Dbsets
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        // Order System Dbsets
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}