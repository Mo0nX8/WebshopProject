using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Webshop.EntityFramework;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Carts;
using Webshop.EntityFramework.Managers.Order;
using Webshop.EntityFramework.Managers.Product;
using Webshop.EntityFramework.Managers.User;
using Webshop.Services.Interfaces;
using Webshop.Services.Services.ViewModel;

namespace WebshopWeb.Controllers
{

    public class OrderController : Controller
    {
        
        private readonly ICartManager cartManager;
        private IUserManager userManager;
        private IOrderManager orderManager;
        private readonly IProductManager productManager;
        private GlobalDbContext _context;
        private IConfiguration _config;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailService emailSender;
        public OrderController(ICartManager cartManager, IProductManager productManager, IOrderManager orderManager, GlobalDbContext context, IUserManager userManager, IConfiguration config, IWebHostEnvironment webHostEnvironment, IEmailService emailSender)
        {
            this.cartManager = cartManager;
            this.productManager = productManager;
            this.orderManager = orderManager;
            _context = context;
            this.userManager = userManager;
            _config = config;
            _webHostEnvironment = webHostEnvironment;
            this.emailSender = emailSender;
        }

        public IActionResult Details()
        {
            var userId = HttpContext.Session.GetInt32("UserId").Value;
            var user = userManager.GetUser(userId);
            _context.Entry(user).Reference(u => u.Address).Load();
            return View(user);
        }
        public IActionResult Confirm(string shipping, string payment)
        {
            Orders order = new Orders();
            var cartId = HttpContext.Session.GetInt32("CartId");
            var cart = cartManager.GetCart(cartId);
            var user = userManager.GetUser(HttpContext.Session.GetInt32("UserId").Value);
            var price = 0;
            List<CartItem> cartItems = cart.CartItems.ToList();
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach(var item in cartItems)
            {
                
                var product=productManager.GetProduct(item.ProductId);
                product.Quanity-=item.Quanity;
                _context.StorageData.Update(product);
                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quanity,
                    Price = item.Product.Price,

                };
                orderItems.Add(orderItem);

            }
            _context.CartItems.RemoveRange(_context.CartItems.Where(c => c.CartId == cart.Id));
            cart.CartItems.Clear();

            cart.CartItems = new List<CartItem>();
           
            order.DateOfOrder=DateTime.Now;
            order.UserId = user.Id;
            order.Price =Convert.ToInt32(orderItems.Sum(p=>p.Quantity*p.Price));
            order.OrderItems = orderItems;
            order.PaymentOption = payment;
            order.ShippingOption = shipping;
            _context.Orders.Add(order);
            _context.SaveChanges();
            SendEmail(order,user.EmailAddress);
            return View();
        }
        public IActionResult PlaceOrder(string shipping, string payment)
        {
            var cartId = HttpContext.Session.GetInt32("CartId").Value;
            var cart = cartManager.GetCart(cartId);
            var userId = HttpContext.Session.GetInt32("UserId").Value;
            var user = userManager.GetUser(userId);
            var address = user.Address;
            var addressGoodFormat = address.ZipCode + ", " + address.City + ", " + address.StreetAndNumber;
            decimal shipmentPrice= Convert.ToDecimal(Regex.Replace(shipping, @"\D", ""));

            decimal paymentPrice = 0;
            if(!payment.Contains("Ingyenes"))
            { 
                paymentPrice= Convert.ToDecimal(Regex.Replace(payment, @"\D", ""));
            }

            var model = new OrderSummaryViewModel
            {
                CustomerName = user.Address.FullName,
                ShippingAddress = addressGoodFormat,
                ShippingOption = shipping,
                PaymentOption = payment,
                Products = cart.CartItems.Select(item => new OrderProductViewModel
                {
                    Name = item.Product.ProductName,
                    ImageUrl = item.Product.ImageData,
                    Price = item.Product.Price,
                    Quantity = item.Quanity
                }).ToList(),
                TotalPrice = cart.CartItems.Sum(item => item.Product.Price * item.Quanity)+paymentPrice+shipmentPrice 
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult RecordAddress(string name, string city, string zip, string street)
        {
            var userId = HttpContext.Session.GetInt32("UserId").Value;
            var userAddress = new Address()
            {
                FullName=name,
                ZipCode = zip,
                StreetAndNumber = street,
                City = city,
            };
            var user=userManager.GetUser(userId);
            user.Address= userAddress;
            userManager.UpdateUser(user);
            _context.SaveChanges();
            var cartId=HttpContext.Session.GetInt32("CartId").Value; 
            return RedirectToAction("Details");
        }
        public async Task SendEmail(Orders order, string email)
        {
            await emailSender.SendOrderEmailAsync(order, email);
            
        }


    }
}
