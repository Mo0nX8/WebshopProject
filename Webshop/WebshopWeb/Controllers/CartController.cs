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
        /// <summary>
        /// Initializes a new instance of the <see cref="CartController"/> class.
        /// </summary>
        /// <param name="authenticationManager">The authentication manager to manage user authentication.</param>
        /// <param name="cartManager">The cart manager to handle cart-related operations.</param>
        /// <param name="userManager">The user manager to retrieve user-related data.</param>
        public CartController(IAuthenticationManager authenticationManager, ICartManager cartManager, IUserManager userManager)
        {
            this.authenticationManager = authenticationManager;
            this.cartManager = cartManager;
            this.userManager = userManager;
        }
        /// <summary>
        /// Displays the user's shopping cart.
        /// </summary>
        /// <returns>A view displaying the cart items.</returns>
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
        /// <summary>
        /// Removes a product from the user's cart.
        /// </summary>
        /// <param name="productId">The ID of the product to remove.</param>
        /// <returns>A redirect to the cart index page.</returns>
        public IActionResult RemoveFromCart(int productId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            int? cartId = HttpContext.Session.GetInt32("CartId");
            cartManager.RemoveItemFromCart(cartId, productId);
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Updates the quantity of a product in the user's cart.
        /// </summary>
        /// <param name="productId">The ID of the product to update.</param>
        /// <param name="newQuanity">The new quantity of the product.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
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
                cartitem.Quantity =newQuanity;
                cartManager.SaveCart(user.Cart.Id);
                return Ok();
            }
            return BadRequest("Nincs kosár.");  
        }
        /// <summary>
        /// Retrieves the number of items in the user's cart.
        /// </summary>
        /// <returns>A JSON result containing the item count in the cart.</returns>
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
