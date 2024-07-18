using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;
using VehicleRentalProject.Models;
using VehicleRentalProject.Repositories.Infrastructure;
using VehicleRentalProject.Web.Models.ViewModels.ApplicationUserViewModels;
using VehicleRentalProject.Web.Models.ViewModels.Order;
using VehicleRentalProject.Web.Utility;

namespace VehicleRentalProject.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderHeaderService _orderHeaderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IWebHostEnvironment _hostingEnvironemnt;
        private readonly IUserService _userService;

        public OrdersController(IOrderHeaderService orderHeaderService,
            IOrderDetailService orderDetailService,
            IWebHostEnvironment hostingEnvironemnt,
            IUserService userService)
        {
            _orderHeaderService = orderHeaderService;
            _orderDetailService = orderDetailService;
            _hostingEnvironemnt = hostingEnvironemnt;
            _userService = userService;
        }
        [Authorize]
        public IActionResult Index(string orderStatus)
        {
            IEnumerable<OrderHeader> orderHeader;
            if (User.IsInRole("Admin"))
            {
                orderHeader =  _orderHeaderService.GetAllOrders();
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim =  claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var userId = claim.Value;
                orderHeader =  _orderHeaderService.GetAllOrdersByUserId(userId);

            }
            switch (orderStatus)
            {
                case "pending":
                    orderHeader = orderHeader.Where(o => o.PaymentStatus == GlobalConfiguration.StatusPending);
                    break;
                case "approved":
                    orderHeader = orderHeader.Where(o => o.PaymentStatus == GlobalConfiguration.StatusApproved);
                    break;
                case "inProcess":
                    orderHeader = orderHeader.Where(o => o.OrderStatus == GlobalConfiguration.StatusInProcess);
                    break;
                case "shipped":
                    orderHeader = orderHeader.Where(o => o.OrderStatus == GlobalConfiguration.StatusShipped);
                    break;
                default:
                    break;
            }
            return View(orderHeader);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var order = new OrderViewModel
            {
                OrderHeader = _orderHeaderService.GetOrderHeader(id),
                OrderDetails = _orderDetailService.GetOrderDetail(id)
            };
            return View(order);
        }
        [HttpPost]
        public IActionResult PayNow(OrderViewModel vm)
        {
            var orderHeader =  _orderHeaderService.GetOrderHeader(vm.OrderHeader.Id);
            var orderDetails  =  _orderDetailService.GetOrderDetail(vm.OrderHeader.Id);
            var domain = "https://localhost:5198/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"customer/carts/ordersuccess?id={vm.OrderHeader.Id}",
                CancelUrl = domain + $"customer/carts/Index",
            };

            foreach (var item in orderDetails)
            {

                var lineItemsOptions = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.rentalTotal * 100),
                        Currency = "INR",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Vehicle.VehicleName,
                        },

                    },
                    Quantity = orderDetails.Count(),


                };
                options.LineItems.Add(lineItemsOptions);
            }

            var service = new SessionService();
            Session session = service.Create(options);
            _orderHeaderService.UpdateStatus(vm.OrderHeader.Id, session.Id, session.PaymentIntentId);
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

        }
        [HttpPost]
         public IActionResult CancelOrder(OrderViewModel vm)
        {
            var orderHeader = _orderHeaderService.GetOrderHeader(vm.OrderHeader.Id);
           // var orderDetails = _orderDetailService.GetOrderDetail(vm.OrderHeader.Id);
            if (orderHeader.PaymentStatus == GlobalConfiguration.StatusApproved)
            {
                var refund = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeader.PaymentIntentId
                };
                var service = new RefundService();
                Refund Refund = service.Create(refund);
                _orderHeaderService.UpdateOrderStatus(orderHeader.Id, GlobalConfiguration.StatusCancelled, GlobalConfiguration.StatusRefund);

            }
            else
            {
                _orderHeaderService.UpdateOrderStatus(orderHeader.Id, GlobalConfiguration.StatusCancelled, GlobalConfiguration.StatusCancelled);
            }

            
            TempData["success"] = "Order Cancelled";
            return RedirectToAction("Details", "Orders", new { id = vm.OrderHeader.Id });

        }
        [HttpGet]
        public IActionResult UpdateUserDetail(string userId)
        {
            UserDetailViewModel vm = new UserDetailViewModel();
            vm.UserId = userId;
            return View(vm);

        }
        [HttpPost]
        public async Task<IActionResult> UpdateUserDetail(UserDetailViewModel vm)

        {
            if (vm.DrivingLicense!=null || vm.PhotoProfId!=null)
            {

              var dlFilePath =   AssignFileName(vm.DrivingLicense,"DLPhoto");
                var photoFilePath = AssignFileName(vm.PhotoProfId, "IDPhoto");
                var user = new UserDetail
                {
                    UserId = vm.UserId,
                    DrivingLicense = dlFilePath,
                    PhotoProfId = photoFilePath,
                    PhoneNumber =  vm.PhoneNumber
                };
                await _userService.AddUserDetail(user);
                return RedirectToAction("Index");              
            }
            return View(vm);

        }

        private string AssignFileName(IFormFile file, string ContainerName)
        {
            string Dir = Path.Combine(_hostingEnvironemnt.WebRootPath, ContainerName);
            string FileExtention = Path.GetExtension(file.FileName);
            string FileName = $"{Guid.NewGuid()}{FileExtention}";
            string FilePath = Path.Combine(Dir, FileName);
            using (var fileStream = new FileStream(FilePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            return FileName;
        }



    }
    }

