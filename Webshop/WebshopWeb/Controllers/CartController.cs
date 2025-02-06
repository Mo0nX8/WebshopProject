using Microsoft.AspNetCore.Mvc;
using Webshop.Services.Interfaces_For_Services;

namespace WebshopWeb.Controllers
{
    public class CartController : Controller
    {
        private readonly IAuthenticationManager authenticationManager;

        public CartController(IAuthenticationManager authenticationManager)
        {
            this.authenticationManager = authenticationManager;
        }

        public IActionResult Index()
        {
            ViewBag.IsAuthenticated = authenticationManager.IsAuthenticated;
            return View();
        }
    }
}
