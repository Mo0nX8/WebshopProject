using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webshop.EntityFramework;
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
        private GlobalDbContext _context;
        public OrderController(ICartManager cartManager, IProductManager productManager, IOrderManager orderManager, GlobalDbContext context)
        {
            this.cartManager = cartManager;
            this.productManager = productManager;
            this.orderManager = orderManager;
            _context = context;
        }

        public IActionResult Details()
        {
            var cartId = HttpContext.Session.GetInt32("CartId");
            List<Products> cartItems=cartManager.GetProduct(cartId.Value);
            ViewData["CartId"]=cartId.Value;
            return View(cartItems);
        }
        public IActionResult Confirm()
        {
            Orders order = new Orders();
            var cartId = HttpContext.Session.GetInt32("CartId");
            var cart = cartManager.GetCart(cartId);

            var price = 0;
            List<CartItem> cartItems = cart.CartItems.ToList();
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach(var item in cartItems)
            {
                price += item.Quanity * item.Product.Price;
                var product=productManager.GetProduct(item.ProductId);
                product.Quanity-=item.Quanity;
                _context.StorageData.Update(product);
                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quanity,
                    Price = item.Product.Price

                };
                orderItems.Add(orderItem);

            }
            _context.CartItems.RemoveRange(_context.CartItems.Where(c => c.CartId == cart.Id));
            cart.CartItems.Clear();

            cart.CartItems = new List<CartItem>();
           
            order.DateOfOrder=DateTime.Now;
            order.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            order.Price =price;
            order.OrderItems = orderItems;
            _context.Orders.Add(order);
            _context.SaveChanges();
            return View();
        }
        public IActionResult PlaceOrder(int cartId)
        {
            var cart=cartManager.GetCart(cartId);
            return View(cart);
        }
    }
}
