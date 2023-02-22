using Microsoft.EntityFrameworkCore;
using paramRestfulAPI.Models;

namespace paramRestfulAPI.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) 
        { 
                  
        }

        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coupon>().HasData(
                new Coupon()
                {
                    Id = 1,
                    Name= "200FF",
                    Percent=15,
                    IsUsable = false,
                },
                new Coupon()
                {
                    Id=2,
                    Name = "400FF",
                    Percent=20,
                    IsUsable=true,
                });
        }
    }
}
