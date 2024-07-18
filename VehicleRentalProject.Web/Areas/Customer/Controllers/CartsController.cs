using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;
using VehicleRentalProject.Models;
using VehicleRentalProject.Repositories.Infrastructure;
using VehicleRentalProject.Web.Models.ViewModels;
using VehicleRentalProject.Web.Utility;

namespace VehicleRentalProject.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartsController : Controller
    {
        private ICartService _cartService;
        private IUserService _userService;
        private IOrderHeaderService _orderHeaderService;
        private IOrderDetailService _orderDetailService;
        public CartsController(ICartService cartService, IUserService userService, IOrderHeaderService orderHeaderService, IOrderDetailService orderDetailService)
        {
            _cartService = cartService;
            _userService = userService;
            _orderHeaderService = orderHeaderService;
            _orderDetailService = orderDetailService;
        }

        public async Task<IActionResult> Index()
        {
            var calimsIdentity = (ClaimsIdentity)User.Identity;
            var calim = calimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var cartList = await _cartService.GetCartItems(calim.Value);
            var vm = new CartVM
            {
                ListOfCart = cartList,
                OrderHeader = new VehicleRentalProject.Models.OrderHeader()
            };
            foreach (var item in cartList)
            {
                vm.OrderHeader.OrderTotal += item.TotalAmount;
            }
            return View(vm);
        }

      public async Task<IActionResult> Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
           
            var cartList = await _cartService.GetCartItems(claims.Value);
            var vm = new CartVM()
            {
                ListOfCart = cartList,
                OrderHeader = new VehicleRentalProject.Models.OrderHeader()
            };
            var user = _userService.GetApplicationUser(claims.Value);
            vm.OrderHeader.Address = user.Address;
            vm.OrderHeader.FullName = user.FullName;
            vm.OrderHeader.ApplicationUser = user;
            foreach (var item in cartList)
            {
                vm.OrderHeader.OrderTotal += item.TotalAmount;
            }
            return View(vm);

        }

        [HttpPost]
        public async Task<IActionResult> Summary(CartVM vm)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var cartList = await _cartService.GetCartItems(claims.Value);
            vm.ListOfCart = cartList;
            vm.OrderHeader.OrderStatus = GlobalConfiguration.StatusPending;
            vm.OrderHeader.PaymentStatus = GlobalConfiguration.StatusPending;
            vm.OrderHeader.DateOfOrder = DateTime.Now;
            vm.OrderHeader.ApplicationUserId = claims.Value;

            foreach (var item in vm.ListOfCart)
            {
                vm.OrderHeader.OrderTotal += (item.TotalAmount);
            }
            _orderHeaderService.Insert(vm.OrderHeader);
            foreach (var item in vm.ListOfCart)
            {
                var OrderDetail = new OrderDetail
                {
                    OrderHeaderId = vm.OrderHeader.Id,
                    VehicleId = item.VehicleId,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    rentalTotal = item.TotalAmount
                };
                _orderDetailService.Insert(OrderDetail);
            }
            await _cartService.ClearCart(claims.Value);

          //  Card Details Here
           var domain = "http://localhost:5198/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"customer/carts/ordersuccess?id={vm.OrderHeader.Id}",
                CancelUrl = domain + $"customer/carts/Index",
            };

            foreach (var item in vm.ListOfCart)
            {

                var lineItemsOptions = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.TotalAmount * 100),
                        Currency = "USD",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Vehicle.VehicleName,
                        },

                    },
                    Quantity = vm.ListOfCart.Count(),
                   
                    
                };
                options.LineItems.Add(lineItemsOptions);
            }

            var service = new SessionService();
            Session session = service.Create(options);
            _orderHeaderService.UpdateStatus(vm.OrderHeader.Id, session.Id, session.PaymentIntentId);
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
           
        }
        public IActionResult ordersuccess(int id)
        {
            var orderHeader =  _orderHeaderService.GetOrderHeader(id);
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);
            if (session.PaymentStatus == "paid")
            {
                _orderHeaderService.UpdateStatus(orderHeader.Id, session.Id, session.PaymentIntentId);
                _orderHeaderService.UpdateOrderStatus(orderHeader.Id, GlobalConfiguration.StatusApproved, GlobalConfiguration.StatusApproved);

            }

            return View(id);
        }


    }

}
