using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Implementations;
using Webshop.EntityFramework.Managers.Interfaces.Cart;
using Webshop.EntityFramework;

namespace Webshop.UnitTests
{
    public class CartManagerTests
    {
        private Mock<DbSet<ShoppingCart>> _mockCarts;
        private Mock<GlobalDbContext> _mockContext;
        private CartManager cartManager;
        [SetUp]
        public void Setup()
        {
            _mockCarts = new Mock<DbSet<ShoppingCart>>();
            _mockContext = new Mock<GlobalDbContext>();
            _mockContext.Setup(x=>x.Carts).Returns(_mockCarts.Object);
            cartManager=new CartManager(_mockContext.Object);
        }
        [Test]
        public void TestThatAddCartShouldAddCartToDatabaseAndSaveIt()
        {
            var cart = new ShoppingCart
            {
                UserId = 1
            };
            cartManager.AddCart(cart);

            _mockCarts.Verify(x=>x.Add(cart),Times.Once);
            _mockContext.Verify(x=>x.SaveChanges(),Times.Once);
        }
    }
}
