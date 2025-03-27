using Microsoft.EntityFrameworkCore;
using ProductAPI.Models;

namespace ProductAPI.Data 
{
    public class AppDBContext : DbContext {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }
        public DbSet<Product> Products {get; set;}
        public DbSet<Review> Reviews {get; set;}
        public DbSet<Cart> Carts {get; set;}
        public DbSet<Ticket> Tickets {get; set;}
        public DbSet<UserProfile> UserProfiles {get; set;}
        public DbSet<Wishlist> Wishlists {get; set;}
        public DbSet<Order> Orders {get; set;}
        public DbSet<OrderItem> OrderItems {get; set;}


    

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
            .HasMany(c => c.Reviews)
            .WithOne(e => e.Product)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
           
            base.OnModelCreating(modelBuilder);
        }
    }
}