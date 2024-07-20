using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentalProject.Models;

namespace VehicleRentalProject.Repositories
{
    public class CarContext : IdentityDbContext<ApplicationUser>
    {
        public CarContext(DbContextOptions<CarContext> options):
            base(options)
       
        {
            
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
       

       

    }
    public class CarContextFactory : IDesignTimeDbContextFactory<CarContext>
    {
        public CarContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CarContext>();
            optionsBuilder.UseSqlServer("Server=SA;Database=VehicleDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");

            return new CarContext(optionsBuilder.Options);
        }
    }
}
