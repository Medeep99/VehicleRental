using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentalProject.Web.Utility;

namespace VehicleRentalProject.Web.Models.ViewModels.Vehicle
{
    public class ListVehicleViewModel
    {
        public IEnumerable<VehicleViewModel> VehicleList { get; set; }
        public PageInfo PageInfo { get; set; }
        public string SearchingText { get; set; }

    }
}
