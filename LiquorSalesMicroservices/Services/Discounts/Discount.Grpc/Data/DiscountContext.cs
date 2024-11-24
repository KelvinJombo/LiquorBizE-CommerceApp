using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data
{
    public class DiscountContext : DbContext
    {
        public DbSet<Coupon> Coupons { get; set; } = default!;
        public DiscountContext(DbContextOptions<DiscountContext> options) : base(options)
        {            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Coupon>().HasData(
                new Coupon { Id = 1, ProductName = "Fanta", Description = "Xmas Bonus", Amount = 750 },
                new Coupon { Id = 2, ProductName = "Sprite", Description = "Xmas Bonus", Amount = 750 },
                new Coupon { Id = 3, ProductName = "NutriYo", Description = "Xmas Bonus", Amount = 750 },
                new Coupon { Id = 4, ProductName = "Gulder Lager Beer", Description = "Xmas Bonus", Amount = 1500 },
                new Coupon { Id = 5, ProductName = "Star Lager Beer", Description = "Xmas Bonus", Amount = 1500 }
            );
        }


    }
}
