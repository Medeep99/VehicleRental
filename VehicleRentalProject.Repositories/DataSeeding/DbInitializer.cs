using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentalProject.Models;
using VehicleRentalProject.Repositories.Utility;

namespace VehicleRentalProject.Repositories.DataSeeding
{
    public class DbInitializer : IDbInitializer
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private CarContext _context;

        public DbInitializer(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, 
            CarContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Any())
                {
                    _context.Database.Migrate();
                }

            }
            catch (Exception)
            {

                throw;
            }
            if (!_roleManager.RoleExistsAsync(FD.Admin_Role)
                .GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(FD.Admin_Role))
                 .GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(FD.Customer_Role))
                 .GetAwaiter().GetResult();
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com"                 


                }, "Admin@123").GetAwaiter().GetResult();
                ApplicationUser user = _context.ApplicationUsers
                    .FirstOrDefault(x => x.Email == "admin@gmail.com");
                _userManager.AddToRoleAsync(user, "Admin").GetAwaiter().GetResult();

            }
            return;
        }
    }
}
