using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using Webshop.EntityFramework.Data;
using Webshop.Services.Interfaces;

namespace Webshop.Services.Services.Email
{
     /// <summary>
     /// This is the implementation for <see cref="IEmailService"/>.
     /// Provides methods for handling email sending.
     /// </summary>>
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly string _templatePath;
        /// <summary>
        /// Constructor for initializing <see cref="EmailService"/> with configuration and optional template path.
        /// </summary>
        /// <param name="config">Application configuration settings.</param>
        /// <param name="templatePath">Optional path to the HTML email template.</param>
        public EmailService(IConfiguration config, string templatePath=null)
        {
            _config = config;
            _templatePath= templatePath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", "emailTemplate.html");
        }
        /// <summary>
        /// Sends a detailed order confirmation email with product images and total price.
        /// </summary>
        /// <param name="order">The order to include in the email.</param>
        /// <param name="email">The recipient's email address.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SendOrderEmailAsync(Orders order, string email)
        {
            string smtpServer = _config["SmtpSettings:Host"];
            int smtpPort = Convert.ToInt32(_config["SmtpSettings:Port"]);
            string senderEmail = _config["SmtpSettings:User"];
            string senderPassword = _config["SmtpSettings:Password"];
            string recipientEmail = email;

            string htmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", "emailTemplate.html");
            string htmlBody = File.Exists(_templatePath) ? File.ReadAllText(_templatePath) : "<html><body><p>Order Confirmation</p></body></html>";

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
                    <td>{item.Price*item.Quantity:C}</td>
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
                    await smtpClient.SendMailAsync(mail);
                }
            }
        }
        /// <summary>
        /// Sends a password reset email or other plain text email.
        /// </summary>
        /// <param name="email">The recipient's email address.</param>
        /// <param name="subject">The email subject line.</param>
        /// <param name="body">The plain text content of the email.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SendResetEmailAsync(string email, string subject, string body)
        {
            string smtpServer = _config["SmtpSettings:Host"];
            int smtpPort = Convert.ToInt32(_config["SmtpSettings:Port"]);
            string senderEmail = _config["SmtpSettings:User"];
            string senderPassword = _config["SmtpSettings:Password"];
            string recipientEmail = email;



            using (MailMessage mail = new MailMessage(senderEmail, recipientEmail))
            {
                mail.Subject = subject;
                mail.IsBodyHtml = false;

                mail.Body = body;

                using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    await smtpClient.SendMailAsync(mail);
                }
            }
        }
    }
}
