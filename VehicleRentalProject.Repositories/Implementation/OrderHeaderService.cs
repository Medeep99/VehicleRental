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
    public class OrderHeaderService : IOrderHeaderService
    {
        private CarContext _context;

        public OrderHeaderService(CarContext context)
        {
            _context = context;
        }

        public IEnumerable<OrderHeader> GetAllOrders()
        {
           var orders =_context.OrderHeaders.Include(x=>x.ApplicationUser).ToList();
            return orders;
        }

        public IEnumerable<OrderHeader> GetAllOrdersByUserId(string userId)
        {
            var orders = _context.OrderHeaders.Where(x=>x.ApplicationUserId==userId).Include(x => x.ApplicationUser).ToList();
            return orders;
        }

        public OrderHeader GetOrderHeader(int id)
        {
            return _context.OrderHeaders.Include(x=>x.ApplicationUser).FirstOrDefault(x=>x.Id==id);
        }

        public void Insert(OrderHeader orderHeader)
        {
            _context.OrderHeaders.Add(orderHeader);
            _context.SaveChanges();
        }

        public void UpdateOrderStatus(int orderHeaderId, string OrderStatus, string paymentStatus)
        {
            var orderHeader =  _context.OrderHeaders.Find(orderHeaderId);
            orderHeader.OrderStatus = OrderStatus;
            orderHeader.PaymentStatus= paymentStatus;
            _context.SaveChanges();
        }

        public void UpdateStatus(int orderHeaderId, string sessionId, string paymentIntentId)
        {
            var orderHeader =  _context.OrderHeaders.Find(orderHeaderId);
            orderHeader.DateofPayment = DateTime.Now;
            orderHeader.PaymentIntentId = paymentIntentId;
            orderHeader.SessionId = sessionId;
            _context.SaveChanges();

        }
    }
}
