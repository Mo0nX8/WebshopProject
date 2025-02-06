using Microsoft.AspNetCore.Mvc;
using Webshop.EntityFramework.Managers.Interfaces.Cart;
using Webshop.Services.Interfaces_For_Services;

namespace WebshopWeb.Controllers
{
    public class CartController : Controller
    {
        private readonly IAuthenticationManager authenticationManager;
        private ICartManager cartManager;

        public CartController(IAuthenticationManager authenticationManager, ICartManager cartManager)
        {
            this.authenticationManager = authenticationManager;
            this.cartManager = cartManager;
        }

        public IActionResult Index(int cartId)

        {
            HttpContext.Session.SetInt32("CartId", cartId);
            ViewBag.IsAuthenticated = authenticationManager.IsAuthenticated;
            var cart = cartManager.GetCart(cartId);
            ViewBag.CartId = cartId;
            return View(cart);
        }
    }
}
