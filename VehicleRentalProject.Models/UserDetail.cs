using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentalProject.Models
{
    public class UserDetail
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string DrivingLicense { get; set; }
        [Required]
        public string PhotoProfId { get; set; }
        [Required]
        public string PhoneNumber { get; set; }

    }
}
