using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentalProject.Models;

namespace VehicleRentalProject.Repositories.Infrastructure
{
    public interface IUserService
    {
        ApplicationUser GetApplicationUser(string userId);
        Task<IEnumerable<ApplicationUser>> GetApplicationUserAsync(string adminId);
        Task AddUserDetail(UserDetail userDetail);
    }
}
