using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Webshop.Authenticator.Services.Authenticator;
using Webshop.Services.Interfaces_For_Services;

namespace WebshopWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuthenticationManager authenticationManager;

        public HomeController(IAuthenticationManager authenticationManager)
        {
            this.authenticationManager = authenticationManager;
        }

        public IActionResult Index()
        {
            ViewBag.IsAuthenticated = authenticationManager.IsAuthenticated;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
