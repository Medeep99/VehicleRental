﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentalProject.Models;

namespace VehicleRentalProject.Repositories.Infrastructure
{
    public interface IOrderHeaderService
    {
        void Insert(OrderHeader orderHeader);
        OrderHeader GetOrderHeader(int id);
        IEnumerable<OrderHeader> GetAllOrders();
        IEnumerable<OrderHeader> GetAllOrdersByUserId(string userId);
        void UpdateOrderStatus(int orderHeaderId, string OrderStatus,string paymentStatus);
        void UpdateStatus(int orderHeaderId, string sessionId, string paymentIntentId);
    }
}
