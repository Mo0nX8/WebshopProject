using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;
using Webshop.Services.Services.Email;

namespace Webshop.UnitTests
{
    [TestFixture]
    public class EmailServiceTests
    {
        private Mock<IConfiguration> _configuration;
        private EmailService _service;
        private string fakeTemplatePath;

        [SetUp]
        public void Setup()
        {
            _configuration = new Mock<IConfiguration>();
            fakeTemplatePath = "fake_template.html";
            File.WriteAllText(fakeTemplatePath, "<html><body><p>Order Confirmation</p></body></html>");
            _service =new EmailService( _configuration.Object, fakeTemplatePath);
            _configuration.Setup(config => config["SmtpSettings:Host"]).Returns("smtp.test.com");
            _configuration.Setup(config => config["SmtpSettings:Port"]).Returns("587");
            _configuration.Setup(config => config["SmtpSettings:User"]).Returns("test@test.com");
            _configuration.Setup(config => config["SmtpSettings:Password"]).Returns("password");
        }
        [TearDown]
        public void Cleanup()
        {
            if (File.Exists(fakeTemplatePath))
            {
                File.Delete(fakeTemplatePath);
            }
        }

        [Test]
        public void TestThatSendEmailAsyncShouldNotThrowException()
        {
            
            var order = new Orders
            {
                Id = 123,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Product = new Products
                        {
                            ProductName = "Test Product",
                            ImageData = Encoding.UTF8.GetBytes("fakeImageData")
                        },
                        Quantity = 2,
                        Price = 1500
                    }
                }
            };
            string testEmail = "test@asd.hu";
            Assert.DoesNotThrowAsync(async () => await _service.SendEmailAsync(order, testEmail));
        }
    }
}
