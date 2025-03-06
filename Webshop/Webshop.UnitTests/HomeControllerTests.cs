using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Interfaces.Product;
using Webshop.EntityFramework.Managers.Interfaces.User;
using Webshop.Services.Interfaces_For_Services;
using Webshop.Services.Services.ViewModel;

namespace Webshop.UnitTests
{
    [TestFixture]
    public class HomeControllerTests
    {
        private Mock<IProductManager> _productManagerMock;
        private HomeController _controller;
        private Mock<HttpContext> httpContextMock;
        private Mock<ISession> _sessionMock;
        private Mock<IUserManager> _userManagerMock;
        private Mock<IEncryptManager> _encryptManagerMock;
        [SetUp]
        public void Setup()
        {
            _productManagerMock = new Mock<IProductManager>();
            
            _sessionMock = new Mock<ISession>();
            _userManagerMock = new Mock<IUserManager>();
            httpContextMock= new Mock<HttpContext>();
            _encryptManagerMock = new Mock<IEncryptManager>();
            httpContextMock.Setup(x=>x.Session).Returns(_sessionMock.Object);
            var httpContext = new DefaultHttpContext();
            var controllerContext = new ControllerContext { HttpContext = httpContext };
            _controller = new HomeController(
                null,
                userManager: _userManagerMock.Object,
                null,
                encryptManager: _encryptManagerMock.Object,
                null,
                productManager:_productManagerMock.Object
            );
            _controller.ControllerContext.HttpContext=httpContextMock.Object;
        }
        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
        }

        [Test]
        public void TestThatIndexShouldReturnViewIndex()
        {
            _productManagerMock.Setup(p=>p.CountProducts()).Returns(3);
            _productManagerMock.Setup(p => p.GetProducts()).Returns(new List<Products>().AsQueryable());

            var result=_controller.Index();
            Assert.IsInstanceOf<ViewResult>(result);
        }
        [Test]
        public void TestThatIndexShouldReturnProductFilterViewModel()
        {
            _productManagerMock.Setup(p => p.CountProducts()).Returns(3);
            _productManagerMock.Setup(p => p.GetProducts()).Returns(new List<Products>().AsQueryable());

            var result = _controller.Index() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ProductFilterViewModel>(result.Model);
        }
        [Test]
        public void TestThatIndexSetTotalItemsCorrect()
        {
            var products = new List<Products>
            {
                new Products { Id = 1, ProductName = "Laptop", Price = 500 },
                new Products { Id = 2, ProductName = "Mouse", Price = 20 }
            }.AsQueryable();

            _productManagerMock.Setup(p => p.CountProducts()).Returns(products.Count());
            _productManagerMock.Setup(p => p.GetProducts()).Returns(products);

            var result=_controller.Index() as ViewResult;
            var model=result.Model as ProductFilterViewModel;

            Assert.AreEqual(products.Count(), model.TotalItems);
        }
        [Test]
        public void TestThatIndexFilterOutZeroPricedProducts()
        {
            var products = new List<Products>
            {
                new Products { Id = 1, ProductName = "Laptop", Price = 500 },
                new Products { Id = 2, ProductName = "Mouse", Price = 20 },
                new Products { Id = 3, ProductName = "Monitor", Price = 0 }
            }.AsQueryable();

            _productManagerMock.Setup(p => p.CountProducts()).Returns(products.Count());
            _productManagerMock.Setup(p => p.GetProducts()).Returns(products);


            var result = _controller.Index() as ViewResult;
            var model=result?.Model as ProductFilterViewModel;
            
            Assert.IsNotNull(model);
            Assert.IsTrue(model.Products.All(p=>p.Price!=0));
            Assert.AreEqual(2, model.Products.Count());
        }
        [Test]
        public void TestThatIndexCanHandleEmptyProductList()
        {
            _productManagerMock.Setup(p => p.CountProducts()).Returns(0);
            _productManagerMock.Setup(p=>p.GetProducts()).Returns(new List<Products>().AsQueryable());

            var result = _controller.Index() as ViewResult;
            var model = result?.Model as ProductFilterViewModel;

            Assert.AreEqual(0, model.TotalItems);
            Assert.AreEqual(0, model.Products.Count());
        }
        [Test]
        public void TestThatIndexShouldSetViewBagIsAuthenticatedTrue()
        {
            _productManagerMock.Setup(p => p.CountProducts()).Returns(3);
            _productManagerMock.Setup(p => p.GetProducts()).Returns(new List<Products>().AsQueryable());

            _controller.Index();

            Assert.IsTrue((bool)_controller.ViewBag.IsAuthenticated);
        }
        [Test]
        public void TestThatPersonalDataRedirectsToLoginWhenNotAuthenticated()
        {
            _sessionMock.Setup(ctx => ctx.TryGetValue("UserId", out It.Ref<byte[]>.IsAny)).Returns(false);
            _sessionMock.Setup(ctx => ctx.TryGetValue("IsAuthenticated", out It.Ref<byte[]>.IsAny)).Returns(false);

            var result = _controller.PersonalData() as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Login", result.ActionName);
            Assert.AreEqual("Authentication", result.ControllerName);
        }
        [Test]
        public void TestThatWhenUserNotFoundPersonalDataReturnsNotFound()
        {
            var userId = 1;
            byte[] userIdBytes = Encoding.UTF8.GetBytes(userId.ToString());
            byte[] authenticatedBytes = Encoding.UTF8.GetBytes("True");
            _sessionMock.Setup(s => s.TryGetValue("UserId", out userIdBytes)).Returns(true);
            _sessionMock.Setup(s => s.TryGetValue("IsAuthenticated", out authenticatedBytes)).Returns(true);
            _userManagerMock.Setup(u => u.GetUser(It.IsAny<int>())).Returns((UserData)null);
            var result = _controller.PersonalData() as NotFoundResult;
            Assert.IsNotNull(result);
        }
        [Test]
        public void TestThatChangePasswordIsSuccedeedWithValidInputs()
        {
            var model = new PasswordChangeViewModel
            {
                CurrentPassword = "oldPassword123",
                NewPassword = "newPassword123"
            };
            var userId = 1;
            var user = new UserData
            {
                Id = userId,
                Username = "testUser",
                EmailAddress="test@test.hu",
                Password = "hashedOldPassword"
            };
            byte[] userIdBytes = Encoding.UTF8.GetBytes(userId.ToString());
            _sessionMock.Setup(s => s.TryGetValue("UserId", out userIdBytes)).Returns(true);
            _userManagerMock.Setup(u => u.GetUser(It.IsAny<int>())).Returns(user);
            _encryptManagerMock.Setup(e => e.Hash(It.IsAny<string>())).Returns("hashedOldPassword");

            var result = _controller.ChangePassword(model) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

    }
}
