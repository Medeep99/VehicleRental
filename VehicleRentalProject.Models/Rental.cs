using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentalProject.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int TotalPrice { get; set; }
        public bool IsPaid { get; set; }     
        public int VehicleId { get; set; }
        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }
        public string ApplicationUserId { get; set; }
        public RentalStatus RentalStatus { get; set; }

        // Navigation Properties
        public virtual Vehicle Vehicle { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
    public enum RentalStatus
    {
        Requested,
        Approved,
        Rejected,
        Rented,
        Closed
    }
}
