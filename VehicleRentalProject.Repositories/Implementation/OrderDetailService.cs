using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentalProject.Models;
using VehicleRentalProject.Repositories.Infrastructure;

namespace VehicleRentalProject.Repositories.Implementation
{
    public class OrderDetailService : IOrderDetailService
    {
        private CarContext _context;

        public OrderDetailService(CarContext context)
        {
            _context = context;
        }

        public IEnumerable<OrderDetail> GetOrderDetail(int orderHeaderId)
        {
           return _context.OrderDetails.Where(x=>x.OrderHeaderId==orderHeaderId)
                .Include(y=>y.Vehicle).ToList();
        }

        public void Insert(OrderDetail orderDetail)
        {
            _context.OrderDetails.Add(orderDetail);
            _context.SaveChanges();
        }
    }
}
