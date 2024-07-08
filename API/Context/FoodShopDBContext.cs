using UI.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace API.Context
{
    public class FoodShopDBContext : DbContext
    {
        public DbSet<Admin> admins {  get; set; }
        public DbSet<Customer> customer { get; set; }
        public DbSet<CustomerInformation> customerInformation { get; set; }
        public DbSet<FoodType> foodType { get; set; }
        public DbSet<FoodCategory> foodCategory { get; set; }
        public DbSet<Food> foods { get; set; }
        public DbSet<Rating> rating { get; set; }
        public DbSet<Cart> cart { get; set; }
        public DbSet<CartItem> cartItem { get; set; }
        public DbSet<Order> order { get; set; }
        public DbSet<OrderItem> orderItem { get; set; }

        public FoodShopDBContext(DbContextOptions<FoodShopDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
            .Entity<Customer>()
            .ToTable(tb => tb.HasTrigger("AUTO_DELETE_CART"));
            modelBuilder
                .Entity<Order>()
                .ToTable(tb => tb.HasTrigger("PAYMENT"));
            modelBuilder
                .Entity<OrderItem>()
                .ToTable(tb => tb.HasTrigger("DECREASE_QUANTITY_FOOD"));
            modelBuilder
              .Entity<Rating>()
              .ToTable(tb => tb.HasTrigger("RATINGCONFRIM"));
        }
    }
}
