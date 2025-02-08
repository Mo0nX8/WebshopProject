using Microsoft.AspNetCore.Mvc;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Implementations;
using Webshop.EntityFramework.Managers.Interfaces.Cart;
using Webshop.EntityFramework.Managers.Interfaces.Order;
using Webshop.EntityFramework.Managers.Interfaces.Product;

namespace WebshopWeb.Controllers
{
    
    public class OrderController : Controller
    {
        private readonly ICartManager cartManager;
        private IOrderManager orderManager;
        private readonly IProductManager productManager;
        public OrderController(ICartManager cartManager, IProductManager productManager, IOrderManager orderManager)
        {
            this.cartManager = cartManager;
            this.productManager = productManager;
            this.orderManager = orderManager;
        }

        public IActionResult Details()
        {
            var cartId = HttpContext.Session.GetInt32("CartId");
            List<Products> cartItems=cartManager.GetProduct(cartId.Value);
            ViewData["CartId"]=cartId.Value;
            return View(cartItems);
        }
        public IActionResult Confirm(int cartId)
        {
            Orders orders = new Orders();
            

            return View();
        }
        public IActionResult PlaceOrder(int cartId)
        {
            var cart=cartManager.GetCart(cartId);
            return View(cart);
        }
    }
}
