using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Order;

namespace Webshop.UnitTests
{
    [TestFixture]
    public class OrderManagerTests
    {
        private Mock<IOrderRepository> _repositoryMock;
        private OrderManager _manager;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IOrderRepository>();
            _manager=new OrderManager(_repositoryMock.Object);
        }
        [Test]
        public void TestThatAddOrderShouldCallAddOrderMethodOnce()
        {
            var order = new Orders { Id = 1, OrderItems = new List<OrderItem>() };

            _manager.AddOrder(order);

            _repositoryMock.Verify(x => x.AddOrder(order),Times.Once);

        }
        [Test]
        public void TestThatNullOrderShouldNotCallAddOrderMethod()
        {
            _manager.AddOrder(null);

            _repositoryMock.Verify(x=>x.AddOrder(It.IsAny<Orders>()),Times.Never);
        }
        [Test]
        public void TestThatRemoveOrderShouldCallRemoveOrderMethodOnce()
        {
            var order = new Orders { Id = 1, OrderItems = new List<OrderItem>() };
            _manager.RemoveOrder(order);
            _repositoryMock.Verify(x=>x.RemoveOrder(order), Times.Once);
        }
        [Test]
        public void TestThatNullOrderShouldNotCallRemoveOrderMethod()
        {
            _manager.RemoveOrder(null);

            _repositoryMock.Verify(x=>x.RemoveOrder(It.IsAny<Orders>()),Times.Never);
        }
        [Test]
        public void TestThatGetOrdersShouldReturnOrdersAsQueryable()
        {
            var orders = new List<Orders>
            {
                new Orders { Id = 1, UserId = 2 },
                new Orders { Id = 2, UserId = 3 }
            }.AsQueryable();
            _repositoryMock.Setup(x=>x.GetOrders()).Returns(orders);

            var result = _manager.GetOrders().ToList();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(3, result[1].UserId);
            Assert.AreEqual(2, result[0].UserId);
            _repositoryMock.Verify(x=>x.GetOrders(), Times.Once);
        }
        [Test]
        public void TestThatGetOrdersReturnsEmptyListIfNoOrderExists()
        {
            _repositoryMock.Setup(x => x.GetOrders()).Returns(new List<Orders>().AsQueryable());

            var result=_manager.GetOrders().ToList();

            Assert.AreEqual(0, result.Count);
            _repositoryMock.Verify(x => x.GetOrders(), Times.Once);

        }
    }
}
