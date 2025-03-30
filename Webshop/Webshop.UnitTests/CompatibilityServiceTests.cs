using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Product;
using Webshop.Services.Interfaces;
using Webshop.Services.Services.Compatibility;

namespace Webshop.UnitTests
{
    [TestFixture]
    public class CompatibilityServiceTests
    {
        private Mock<IProductManager> _productManager;
        private CompatibilityService _service;

        [SetUp]
        public void Setup()
        {
            _productManager = new Mock<IProductManager>();
            _service=new CompatibilityService(_productManager.Object);
        }
        [Test]
        public void TestThatGetAllProductsShouldReturnProducts()
        {
            var testProducts = new List<Products>
            {
                new Products { Id = 1, ProductName = "Test CPU", Tags = new string[] { "cpu" }, Price = 100 },
                new Products { Id = 2, ProductName = "Test Motherboard", Tags = new string[] { "alaplap" }, Price = 100 },
                new Products { Id = 3, ProductName = "Test RAM", Tags = new string[] { "memória" }, Price = 100 },
                new Products { Id = 4, ProductName = "Test non-pc items", Tags = new string[] { "other" }, Price = 100 }
            };

            _productManager.Setup(x=>x.GetProducts()).Returns(testProducts.AsQueryable());

            var result=_service.GetAllProducts().ToList();

            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result.Any(p => p.Name == "Test CPU" && p.Category == "Processzor"));
            Assert.IsTrue(result.Any(p => p.Name == "Test Motherboard" && p.Category == "Alaplap"));
            Assert.IsTrue(result.Any(p => p.Name == "Test RAM" && p.Category == "Memória"));
        }
        [Test]
        public void TestThatFilterProductsShouldReturnCompatibleProducts()
        {
            var testProducts = new List<Products>
            {
                new Products { Id = 1, ProductName = "CPU AM4", Tags = new string[] { "AM4", "cpu" }, Price = 100 },
                new Products { Id = 2, ProductName = "Motherboard AM4", Tags = new string[] { "AM4", "alaplap" }, Price = 150 },
                new Products { Id = 3, ProductName = "RAM DDR4", Tags = new string[] { "DDR4", "memória" }, Price = 80 },
                new Products { Id = 4, ProductName = "Case ATX", Tags = new string[] { "ATX", "Gépház" }, Price = 120 },
                new Products { Id = 5, ProductName = "SSD", Tags = new string[] { "SSD" }, Price = 60 }
            };

            _productManager.Setup(x => x.GetProducts()).Returns(testProducts.AsQueryable());

            var result = _service.FilterProducts(motherboardId: 2, cpuId: null, ramId: null, caseId: null).ToList();

            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result.Any(p => p.Name == "CPU AM4"));
            Assert.IsTrue(result.Any(p => p.Name == "Motherboard AM4"));
            Assert.IsTrue(result.Any(p => p.Name == "SSD"));
        }
    }
}
