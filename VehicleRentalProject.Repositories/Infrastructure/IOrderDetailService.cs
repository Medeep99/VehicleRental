using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentalProject.Models;

namespace VehicleRentalProject.Repositories.Infrastructure
{
    public interface IOrderDetailService
    {
        void Insert(OrderDetail orderDetail);
        IEnumerable<OrderDetail> GetOrderDetail(int orderHeaderId);

    }
}
