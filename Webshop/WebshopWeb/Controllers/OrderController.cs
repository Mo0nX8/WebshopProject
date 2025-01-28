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
        private readonly IProductManager productManager;
        public OrderController(ICartManager cartManager, IProductManager productManager)
        {
            this.cartManager = cartManager;
            this.productManager = productManager;
        }

        public IActionResult Details(Cart cart)
        {
            var cartItems=cartManager.GetProduct(cart.Id);
            return View(cartItems);
        }
        public IActionResult Confirm()
        {
            return View();
        }
        public IActionResult PlaceOrder(Cart cart)
        {
            return View(cart);
        }
    }
}
