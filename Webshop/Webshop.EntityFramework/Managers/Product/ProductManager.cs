using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Product
{
    /// <summary>
    /// This is the implementation for IProductManager.
    /// Manages product-related operations such as adding, retrieving, and counting products.
    /// </summary>
    public class ProductManager : IProductManager
    {
        private IProductRepository _productRepository;

        /// <summary>
        /// Initializes a new instance of the ProductManager class with the provided IProductRepository.
        /// </summary>
        /// <param name="productRepository">The repository used for interacting with the products data.</param>
        public ProductManager(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Adds a new product to the repository.
        /// </summary>
        /// <param name="storage">The product to be added.</param>
        public void AddProduct(Products storage)
        {
            // Ensure the product is not null before attempting to add it.
            if (storage is not null)
            {
                _productRepository.AddProduct(storage);
            }
        }

        /// <summary>
        /// Counts the total number of products in the repository.
        /// </summary>
        /// <returns>The total number of products.</returns>
        public int CountProducts()
        {
            // Returns the count of products from the repository.
            return _productRepository.CountProducts();
        }

        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product to retrieve.</param>
        /// <returns>The product matching the given id, or null if not found.</returns>
        public Products GetProduct(int id)
        {
            // If the provided id is not null, attempt to retrieve the product from the repository.
            if (id != null)
            {
                return _productRepository.GetProduct(id);
            }
            return null;
        }

        /// <summary>
        /// Retrieves all products from the repository.
        /// </summary>
        /// <returns>An IQueryable collection of all products.</returns>
        public IQueryable<Products> GetProducts()
        {
            // Returns all products in the repository as an IQueryable.
            return _productRepository.GetProducts();
        }
    }
}
