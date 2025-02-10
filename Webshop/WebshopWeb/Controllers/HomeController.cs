using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Webshop.Authenticator.Services.Authenticator;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Interfaces.User;
using Webshop.Services.Interfaces_For_Services;
using Webshop.Services.Services.ViewModel;

namespace WebshopWeb.Controllers
{
    public class HomeController : Controller
    {
        private IUserManager userManager;
        private readonly IAuthenticationManager authenticationManager;

        public HomeController(IAuthenticationManager authenticationManager, IUserManager userManager)
        {
            this.authenticationManager = authenticationManager;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {

            ViewBag.IsAuthenticated = authenticationManager.IsAuthenticated;
            return View();
        }
        public IActionResult PersonalData()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            ViewBag.UserId = userId;
            var authenticated = HttpContext.Session.GetString("IsAuthenticated");
            if (authenticated=="False")
            {
                return RedirectToAction("Login", "Authentication");
            }

            var user = userManager.GetUser(Convert.ToInt32(userId));
            if (user == null)
            {
                return NotFound();
            }

            var model = new PersonalDataViewModel
            {
                UserId = Convert.ToInt32(userId),
                UserName = user.Username,
                Email = user.EmailAddress
            };

            return View("PersonalData", model);
        }

    }
}
