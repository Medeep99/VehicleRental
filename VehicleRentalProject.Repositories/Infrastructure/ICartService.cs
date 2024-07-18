using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentalProject.Models;

namespace VehicleRentalProject.Repositories.Infrastructure
{
    public interface ICartService
    {
        Task AddToCart(Cart cart);
        Task<Cart> GetCartItems(string userId,int vehicleId);
        Task<List<Cart>> GetCartItems(string userId);

        //Task RemoveFromCart(int vehicleId, string userId);
        Task ClearCart(string userId);

        //Task<decimal> GetTotalAmount(string userId);
        //Task<int> GetTotalDuration(string userId);


    }
}
