using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentalProject.Models;
using VehicleRentalProject.Repositories.Infrastructure;

namespace VehicleRentalProject.Repositories.Implementation
{
    public class UserService : IUserService
    {
        private CarContext _context;

        public UserService(CarContext context)
        {
            _context = context;
        }

        public ApplicationUser GetApplicationUser(string userId)
        {
            var applicatioUser = _context.ApplicationUsers.Where(x => x.Id == userId).FirstOrDefault();
            return applicatioUser;
        }

        public async Task<IEnumerable<ApplicationUser>> GetApplicationUserAsync(string adminId)
        {
            var users =  await _context.ApplicationUsers.Where(x => x.Id != adminId)
                .ToListAsync();
            return users;
        }

        public async Task AddUserDetail(UserDetail userDetail)
        {
            //var user =  _context.ApplicationUsers.Where(x => x.Id == userId).FirstOrDefault();
            await _context.UserDetails.AddAsync(userDetail);
           await _context.SaveChangesAsync();

        }
    }
}
