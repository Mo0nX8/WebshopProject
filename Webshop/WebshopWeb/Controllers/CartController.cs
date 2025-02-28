using Microsoft.AspNetCore.Mvc;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Interfaces.Cart;
using Webshop.EntityFramework.Migrations;
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

        public IActionResult Index()

        {
            int? cartId = HttpContext.Session.GetInt32("CartId");
            ViewBag.IsAuthenticated = authenticationManager.IsAuthenticated;
            var cart = cartManager.GetCart(cartId);
            var cartItems = new List<CartItem>().ToList();
            if (cart!=null)
            {
                cartItems=cart.CartItems.ToList();
            }
           
            ViewBag.CartId = cartId;
            return View(cartItems);
        }
        public IActionResult RemoveFromCart(int productId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            int? cartId = HttpContext.Session.GetInt32("CartId");
            cartManager.RemoveItemFromCart(cartId, productId);
            return RedirectToAction("Index");
        }


    }
}
