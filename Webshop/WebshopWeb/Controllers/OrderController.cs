using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using Webshop.EntityFramework;
using Webshop.EntityFramework.Data;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using System.Net.Mime;
using Webshop.EntityFramework.Managers.Carts;
using Webshop.EntityFramework.Managers.Order;
using Webshop.EntityFramework.Managers.User;
using Webshop.EntityFramework.Managers.Product;

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
        public OrderController(ICartManager cartManager, IProductManager productManager, IOrderManager orderManager, GlobalDbContext context, IUserManager userManager, IConfiguration config, IWebHostEnvironment webHostEnvironment)
        {
            this.cartManager = cartManager;
            this.productManager = productManager;
            this.orderManager = orderManager;
            _context = context;
            this.userManager = userManager;
            _config = config;
            _webHostEnvironment = webHostEnvironment;
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
            string smtpServer = _config["SmtpSettings:Host"];
            int smtpPort = Convert.ToInt32(_config["SmtpSettings:Port"]);
            string senderEmail = _config["SmtpSettings:User"];
            string senderPassword = _config["SmtpSettings:Password"];
            string recipientEmail = "Anakinka2323@gmail.com";

            string htmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", "emailTemplate.html");
            string htmlBody = System.IO.File.ReadAllText(htmlFilePath);

            StringBuilder orderItemsHtml = new StringBuilder();
            int imageIndex = 1;

            using (MailMessage mail = new MailMessage(senderEmail, recipientEmail))
            {
                mail.Subject = "Rendelés #" + order.Id;
                mail.IsBodyHtml = true;

                foreach (var item in order.OrderItems)
                {
                    string contentId = $"image{imageIndex}";

                    orderItemsHtml.AppendLine($@"
                <tr>
                    <td><img src='cid:{contentId}' width='100' height='100'></td>
                    <td>{item.Product.ProductName}</td>
                    <td>{item.Quantity}</td>
                    <td>{item.Price:C}</td>
                </tr>");

                    MemoryStream imageStream = new MemoryStream(item.Product.ImageData);
                    Attachment inline = new Attachment(imageStream, "image.jpg", "image/jpeg")
                    {
                        ContentId = contentId,
                        ContentDisposition = { Inline = true, DispositionType = DispositionTypeNames.Inline }
                    };
                    mail.Attachments.Add(inline);

                    imageIndex++;
                }

                var totalPrice = order.OrderItems.Sum(item => item.Quantity * item.Price);
                htmlBody = htmlBody.Replace("{{ORDER_ITEMS}}", orderItemsHtml.ToString());
                htmlBody = htmlBody.Replace("{{TOTAL_PRICE}}", totalPrice + " Ft");

                mail.Body = htmlBody;
                    
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
