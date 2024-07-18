using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;
using VehicleRentalProject.Models;
using VehicleRentalProject.Repositories.Infrastructure;
using VehicleRentalProject.Web.Models.ViewModels.Vehicle;

namespace VehicleRentalProject.Web.Controllers
{
    public class HomeController : Controller
    {
        private IVehicleRepository _vehicleRepo;
        private IMapper _mapper;
        private IUserService _userService;
        private ICartService _cartService;

        public HomeController(IVehicleRepository vehicleRepo,
            IMapper mapper,
            IUserService userService,
            ICartService cartService)
        {
            _vehicleRepo = vehicleRepo;
            _mapper = mapper;
            _userService = userService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var vehicles = _vehicleRepo.GetVehicles().GetAwaiter().GetResult().
                ToList().Where(x=>!x.IsDeleted && x.IsAvailable);
            var vm = _mapper.Map<List<VehicleViewModel>>(vehicles);
            return View(vm);
        }
        public async Task<IActionResult> Details(int id)
        {
            var vehicle = await _vehicleRepo.GetVehicleById(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            var vm = _mapper.Map<VehicleDetailsViewModel>(vehicle);
            return View(vm);
        }
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Order(SummaryViewModel vm)
        {
            var domain = "http://localhost:5198";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    
                    Price =vm.TotalAmount.ToString()
                    
                  },
                },
                Mode = "payment",
                SuccessUrl = domain + "/Home/success",
                CancelUrl = domain + "/Home/cancel",
            };
            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
            //return View();
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Details(VehicleDetailsViewModel vm)
        {
            var claimsIdentity =  (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
           var applicationUser =  _userService.GetApplicationUser(claims.Value);

            var cart = await _cartService.GetCartItems(claims.Value, vm.Id);
            if (cart==null)
            {
                if (ModelState.IsValid)
                {
                    Cart cartObj = new Cart();
                    TimeSpan duration = (TimeSpan)(vm.EndDate - vm.StartDate);
                    cartObj.TotalAmount = vm.DailyRate * duration.Days;
                    cartObj.EndDate = vm.EndDate;
                    cartObj.StartDate = vm.StartDate;                    
                    cartObj.TotalDuration = duration.Days;
                    //cartObj.Vehicle.VehicleImage = vm.VehicleImage;
                    cartObj.VehicleId = vm.Id;
                    cartObj.ApplicationUser = applicationUser;
                    await _cartService.AddToCart(cartObj);
                    HttpContext.Session.SetInt32("SessionCart",
                        _cartService.GetCartItems(claims.Value).GetAwaiter().GetResult().Count);
            
                    return RedirectToAction("Index","Home");
                }


            }
            else
            {
                ViewBag.Message = "Vehicle already added to cart";
            }
           
            return View(vm);
        }

        }
}
