using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentalProject.Web.Models.ViewModels.ApplicationUserViewModels
{
    public class UserDetailViewModel
    {
        public string UserId { get; set; }
        public IFormFile DrivingLicense { get; set; }
        public IFormFile PhotoProfId { get; set; }
        public string PhoneNumber { get; set; }



    }
}
