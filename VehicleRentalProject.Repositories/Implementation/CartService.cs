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
    public class CartService : ICartService
    {
        private readonly CarContext _context;

        public CartService(CarContext context)
        {
            _context = context;
        }

        public async Task AddToCart(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();

        }

        public async Task ClearCart(string userId)
        {
          var cartItems =  await _context.Carts.Where(x => x.ApplicationUser.Id == userId).ToListAsync();
            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }

        public async Task<Cart> GetCartItems(string userId, int vehicleId)
        {
            var cart = await _context.Carts.Where(c => c.ApplicationUser.Id == userId && c.VehicleId == vehicleId).FirstOrDefaultAsync();
            return cart;
        }

        public async Task<List<Cart>> GetCartItems(string userId)
        {
          var cartItems =   await _context.Carts.Include(z=>z.Vehicle).Where(x => x.ApplicationUser.Id == userId)
                .ToListAsync();
            return cartItems;
        }
    }
}
