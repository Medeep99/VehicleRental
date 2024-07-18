using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentalProject.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        [Required]
        public int OrderHeaderId { get; set; }
        [ValidateNever]
        public OrderHeader OrderHeader { get; set; }
        [Required]
        public int VehicleId { get; set; }
        [ValidateNever]
        public Vehicle Vehicle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }       
        public decimal rentalTotal { get; set; }
    }
}
