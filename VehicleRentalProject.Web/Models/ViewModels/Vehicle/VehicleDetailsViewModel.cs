using Microsoft.Build.Execution;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentalProject.Web.Validations;

namespace VehicleRentalProject.Web.Models.ViewModels.Vehicle
{
    public class VehicleDetailsViewModel
    {
        public int Id { get; set; }       
        public string VehicleName { get; set; }
        public string VehicleType { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleNumber { get; set; }
        public string VehicleColor { get; set; }
        public string VehicleDescription { get; set; }
        public string VehicleImage { get; set; }
        public decimal DailyRate { get; set; }
        [Display(Name = "Start Date")]
        [Required(ErrorMessage = "Please select a Start Date.")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [Required(ErrorMessage = "Please select an End Date.")]
        [DataType(DataType.Date)]
        [CustomDateRange(ErrorMessage = "End Date must be greater than or equal to Start Date.")]
        public DateTime? EndDate { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
