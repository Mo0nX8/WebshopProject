using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;
using Webshop.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Webshop.Services.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly string _templatePath;

        public EmailService(IConfiguration config, string templatePath=null)
        {
            _config = config;
            _templatePath= templatePath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", "emailTemplate.html");
        }

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
                    await smtpClient.SendMailAsync(mail);
                }
            }
        }

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
