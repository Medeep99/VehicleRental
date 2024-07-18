using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentalProject.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }        
        public Vehicle Vehicle { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int TotalDuration { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}
