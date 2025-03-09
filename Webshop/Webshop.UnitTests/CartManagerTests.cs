using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Implementations;
using Webshop.EntityFramework.Managers.Interfaces.Cart;
using Webshop.EntityFramework;
using Webshop.EntityFramework.Managers.Interfaces.Carts;

namespace Webshop.UnitTests
{
    public class CartManagerTests
    {
        private Mock<ICartRepository> _cartRepositoryMock;
        private CartManager _cartManager;   
        [SetUp]
        public void Setup()
        {
            _cartRepositoryMock= new Mock<ICartRepository>();
            _cartManager = new CartManager(_cartRepositoryMock.Object);
        }
        [Test]
        public void TestThatAddCartShouldCallAddCartMethod()
        {
            var cart=new ShoppingCart { Id=1, UserId=1 };

            _cartManager.AddCart(cart);

            _cartRepositoryMock.Verify(x=>x.AddCart(cart),Times.Once);
        }
        [Test]
        public void TestThatGetCartMethodWithValidIdReturnsCart()
        {
            var cart = new ShoppingCart { Id = 1, UserId = 2 };
            _cartRepositoryMock.Setup(x => x.GetCart(1)).Returns(cart);

            var result = _cartManager.GetCart(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual(2, result.UserId);
        }
        [Test]
        public void TestThatGetCartMethodWithNullIdReturnsNull()
        {
            var result= _cartManager.GetCart(null);
            Assert.IsNull(result);
        }
        [Test]
        public void TestThatAddProductsShouldCallAddProductsMethod()
        {
            var userid = 22;
            var productIds = new List<int> { 1, 2, 3 };

            _cartManager.AddProductsToCart(userid, productIds);

            _cartRepositoryMock.Verify(x => x.AddProductsToCart(userid, productIds), Times.Once);
        }
        [Test]
        public void TestThatGetProductsMethodShouldReturnAListOfProducts()
        {
            var products = new List<Products>
            {
                new Products { Id = 1, ProductName="laptop" },
                new Products { Id = 2, ProductName="egér" }
            };

            _cartRepositoryMock.Setup(x=>x.GetProducts(1)).Returns(products);

            var result = _cartManager.GetProduct(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(2,result.Count());
            Assert.AreEqual("laptop", result[0].ProductName);
            Assert.AreEqual("egér", result[1].ProductName);

        }
        [Test]
        public void TestThatRemoveItemFromCartShouldCallRemoveItemFromCartMethod()
        {
            var cartid = 22;
            var productId = 1;

            _cartManager.RemoveItemFromCart(cartid, productId);
            _cartRepositoryMock.Verify(x => x.RemoveItemFromCart(cartid, productId), Times.Once);
        }
        [Test]
        public void TestThatWithNullCartIdRemoveItemFromCartMethodIsNotCalled()
        {
            int? cartid = null;
            var productId = 3;

            _cartManager.RemoveItemFromCart(cartid,productId);

            _cartRepositoryMock.Verify(x => x.RemoveItemFromCart(It.IsAny<int>(),productId),Times.Never);
        }

    }
}
