using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using Webshop.EntityFramework;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Interfaces.Cart;
using Webshop.EntityFramework.Managers.Interfaces.Order;
using Webshop.EntityFramework.Managers.Interfaces.Product;
using Webshop.EntityFramework.Managers.Interfaces.User;
using System.IO;
using System.Text;

namespace WebshopWeb.Controllers
{

    public class OrderController : Controller
    {
        
        private readonly ICartManager cartManager;
        private IUserManager userManager;
        private IOrderManager orderManager;
        private readonly IProductManager productManager;
        private GlobalDbContext _context;
        public OrderController(ICartManager cartManager, IProductManager productManager, IOrderManager orderManager, GlobalDbContext context, IUserManager userManager)
        {
            this.cartManager = cartManager;
            this.productManager = productManager;
            this.orderManager = orderManager;
            _context = context;
            this.userManager = userManager;
        }

        public IActionResult Details()
        {
            var userId = HttpContext.Session.GetInt32("UserId").Value;
            var user = userManager.GetUser(userId);
            _context.Entry(user).Reference(u => u.Address).Load();
            return View(user);
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
            SendEmail(order);
            return View();
        }
        public IActionResult PlaceOrder(int cartId)
        {
            var cart=cartManager.GetCart(cartId);
            return View(cart);
        }
        [HttpPost]
        public IActionResult RecordAddress(string city, string zip, string street)
        {
            var userId = HttpContext.Session.GetInt32("UserId").Value;
            var userAddress = new Address()
            {
                ZipCode = zip,
                StreetAndNumber = street,
                City = city,
            };
            var user=userManager.GetUser(userId);
            user.Address= userAddress;
            userManager.UpdateUser(user);
            _context.SaveChanges();
            var cartId=HttpContext.Session.GetInt32("CartId").Value; 
            return RedirectToAction("PlaceOrder", new { cartId });
        }
        public void SendEmail(Orders order)
        {
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587; 
            string senderEmail = "dxmarket234@gmail.com";
            string senderPassword = "whga sfrg yjjn lvzu"; 
            string recipientEmail = "anakinka2323@gmail.com";
            string htmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", "emailTemplate.html");
            string htmlBody = System.IO.File.ReadAllText(htmlFilePath);

            StringBuilder orderItemsHtml = new StringBuilder();
            foreach (var item in order.OrderItems)
            {
                orderItemsHtml.AppendLine($"<tr><td>{item.Product.ProductName}</td><td>{item.Quantity}</td><td>{item.Price:C}</td></tr> <tr><td></td></tr>");
            }
            var totalPrice = order.OrderItems.Sum(item => item.Quantity * item.Price);
            var totalPriceString = totalPrice+"Ft";
            htmlBody = htmlBody.Replace("{{ORDER_ITEMS}}", orderItemsHtml.ToString());
            htmlBody = htmlBody.Replace("{{TOTAL_PRICE}}", totalPriceString);
            using (MailMessage mail = new MailMessage(senderEmail, recipientEmail))
            {
                mail.Subject = "Rendelés"+"#"+order.Id;
                mail.Body = htmlBody;
                mail.IsBodyHtml = true;

                using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                    smtpClient.EnableSsl = true; 
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.Send(mail);
                }
            }
        }


    }
}
