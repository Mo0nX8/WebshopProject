using Moq;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Implementations;
using Webshop.EntityFramework.Managers.Interfaces.Product;

namespace Webshop.UnitTests
{
    [TestFixture]
    public class ProductManagerTests
    {
        private Mock<IProductRepository> _productRepositoryMock;
        private ProductManager _productManager;

        [SetUp]
        public void Setup()
        {
            _productRepositoryMock= new Mock<IProductRepository>();
            _productManager=new ProductManager( _productRepositoryMock.Object );
        }
        [Test]
        public void TestThatAddProductCallsAddProductMethodWhenAddingAValidProduct()
        {
            var product = new Products { Id = 1, ProductName = "Laptop", Price = 1000 };
            _productRepositoryMock.Setup(x => x.AddProduct(It.IsAny<Products>())).Verifiable();

            _productManager.AddProduct(product);

            _productRepositoryMock.Verify(x=> x.AddProduct(It.Is<Products>(y=>y==product)),Times.Once);
        }
        [Test]
        public void TestThatWhenProductIsNotNullAddProductShouldNotBeCalled()
        {
            _productRepositoryMock.Setup(x => x.AddProduct(It.IsAny<Products>())).Verifiable();
            _productManager.AddProduct(null);

            _productRepositoryMock.Verify(x=>x.AddProduct(It.IsAny<Products>()),Times.Never);
        }
        [Test]
        public void TestThatCountProductsMethodShouldReturnTheCorrectCountOfProducts()
        {
            var productCount = 6;
            _productRepositoryMock.Setup(x => x.CountProducts()).Returns(productCount);

            var result = _productManager.CountProducts();

            Assert.AreEqual(productCount, result);
        }
        [Test]
        public void TestThatCountProductsMethodReturnsZeroWhenProductCountIsZero()
        {
            _productRepositoryMock.Setup(x=> x.CountProducts()).Returns(0);
            var result = _productManager.CountProducts();
            Assert.AreEqual(0, result);
        }
        [Test]
        public void TestThatWithValidIdGivenGetProductMethodReturnsProduct()
        {
            var productId = 1;
            var product = new Products { Id = 1, ProductName="Laptop", Price=1200 };

            _productRepositoryMock.Setup(x=>x.GetProduct(productId)).Returns(product);

            var result=_productManager.GetProduct(productId);

            Assert.AreEqual(product, result);
        }
        [Test]
        public void TestThatWithInvalidIdGetProductReturnsNull()
        {
            var productId = 1111;
            _productRepositoryMock.Setup(x => x.GetProduct(It.IsAny<int>())).Returns((Products)null);

            var result = _productManager.GetProduct(productId);

            Assert.IsNull(result);
        }
        [Test]
        public void TestThatGetProductsShouldReturnAListOfProductsAsQueryable()
        {
            var products = new List<Products>
            {
                new Products {Id=1,ProductName="Laptop",Price=1000},
                new Products {Id=2,ProductName="Egér",Price=1100},
                new Products {Id=3,ProductName="Monitor",Price=1200}
            };
            _productRepositoryMock.Setup(x => x.GetProducts()).Returns(products.AsQueryable());

            var result= _productManager.GetProducts();

            Assert.AreEqual(products.Count(), result.Count());
            Assert.Contains(products[0], result.ToList());
            Assert.Contains(products[1], result.ToList());
            Assert.Contains(products[2], result.ToList());
        }
        [Test]
        public void TestThatWhenNoProductsGetProductsMethodReturnsEmptyList()
        {
            _productRepositoryMock.Setup(x=>x.GetProducts()).Returns(new List<Products>().AsQueryable());

            var result= _productManager.GetProducts();

            Assert.IsEmpty(result);
        }
    }
}
