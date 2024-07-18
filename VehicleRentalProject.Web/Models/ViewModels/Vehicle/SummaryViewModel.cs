using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentalProject.Models;
using VehicleRentalProject.Web.Validations;

namespace VehicleRentalProject.Web.Models.ViewModels.Vehicle
{
    public class SummaryViewModel
    {
        public int Id { get; set; }  
        public string VehicleType { get; set; }        
        public string VehicleNumber { get; set; }
        public string VehicleImage { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime StartDate { get; set; }        
        public DateTime? EndDate { get; set; }
        public int TotalDuration { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}
