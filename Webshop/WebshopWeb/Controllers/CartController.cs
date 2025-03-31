using Microsoft.AspNetCore.Mvc;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Carts;
using Webshop.EntityFramework.Managers.User;
using Webshop.Services.Interfaces;

namespace WebshopWeb.Controllers
{
    public class CartController : Controller
    {
        private readonly IAuthenticationManager authenticationManager;
        private ICartManager cartManager;
        private readonly IUserManager userManager;

        public CartController(IAuthenticationManager authenticationManager, ICartManager cartManager, IUserManager userManager)
        {
            this.authenticationManager = authenticationManager;
            this.cartManager = cartManager;
            this.userManager = userManager;
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
        [HttpPost("api/update/{productId}/{newQuanity}")]
        public IActionResult UpdateQuantity(int productId, int newQuanity)
        {
            var userid=HttpContext.Session.GetInt32("UserId");
            var user = userManager.GetUser(userid.Value);
            if(user==null)
            {
                return BadRequest("Nem vagy bejelentkezve.");
            }
            var cartitem = cartManager.GetCart(user.Cart.Id).CartItems.FirstOrDefault(y=>y.ProductId==productId);

            if(cartitem!=null)
            {
                cartitem.Quanity=newQuanity;
                cartManager.SaveCart(user.Cart.Id);
                return Ok();
            }
            return BadRequest("Nincs kosár.");  
        }
        [HttpGet]
        public JsonResult GetCartCount()
        {
            int? cartId = HttpContext.Session.GetInt32("CartId");
            var cart = cartId!=null ? cartManager.GetCart(cartId.Value) : new ShoppingCart();
            int itemCount = cart.CartItems.Count();
            return Json(new { itemCount = itemCount });
            
        }


    }
}
