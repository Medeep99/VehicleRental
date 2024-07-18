using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VehicleRentalProject.Repositories.Infrastructure;
using VehicleRentalProject.Repositories.Utility;
using VehicleRentalProject.Web.Models.ViewModels.ApplicationUserViewModels;

namespace VehicleRentalProject.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [Authorize(Roles = FD.Admin_Role)]
        public async  Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var users =  await _userService.GetApplicationUserAsync(claim.Value);
            var userViewModel = _mapper.Map<List<UserViewModel>>(users);
            return View(userViewModel);
        }
    }
}
