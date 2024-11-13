﻿using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data
{
    public class DiscountContext : DbContext 
    {

        public DiscountContext(DbContextOptions<DiscountContext> options) : base(options) { }
        
             
        public DbSet<CouponModel> Coupons { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coupon>().HasData(
                new Coupon { Id = 1, ProductName = "Hero Lager", Description = "Hero Discount", Amount = 500 },
                new Coupon { Id = 2, ProductName = "Trophy Lager", Description = "Green bottle", Amount = 400 }
            );
            base.OnModelCreating(modelBuilder);
        }
    }
}