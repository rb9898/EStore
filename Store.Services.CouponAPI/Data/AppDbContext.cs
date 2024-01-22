using Microsoft.EntityFrameworkCore;
using Store.Services.CouponAPI.Models;
namespace Store.Services.CouponAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Coupon>().HasData(new Coupon()
            {
                CouponId = 1,
                CouponCode = "10OFF",
                MinAmount = 20,
                DiscountAmount = 10
            });

            modelBuilder.Entity<Coupon>().HasData(new Coupon()
            {
                CouponId = 2,
                CouponCode = "20OFF",
                MinAmount = 40,
                DiscountAmount = 20
            });

            modelBuilder.Entity<Coupon>().HasData(new Coupon()
            {
                CouponId = 3,
                CouponCode = "30OFF",
                MinAmount = 60,
                DiscountAmount = 30
            });
        }
    }
}
