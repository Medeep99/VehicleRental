using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VehicleRentalProject.Repositories.Infrastructure;

namespace VehicleRentalProject.Web.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private ICartService _cartService;

        public CartViewComponent(ICartService cartService)
        {
            _cartService = cartService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity =  (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            if(userId!=null)
            {
                if (HttpContext.Session.GetInt32("SessionCart") != null)
                {
                    return View(HttpContext.Session.GetInt32("SessionCart"));
                }
                else
                {
                   HttpContext.Session.SetInt32("SessionCart", _cartService.GetCartItems(userId)
                       .GetAwaiter().GetResult().Count);
                    return View(HttpContext.Session.GetInt32("SessionCart"));
                }
            }
            else
            {
                HttpContext.Session.Clear();
            return View(0);
            }
        }
    }
}
