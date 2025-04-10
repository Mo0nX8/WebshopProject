using Microsoft.EntityFrameworkCore;
using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Product
{
    /// <summary>
    /// Repository class for managing product-related database operations.
    /// This includes adding, retrieving, and counting products in the database.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private GlobalDbContext _context;

        /// <summary>
        /// Initializes a new instance of the ProductRepository class with the provided GlobalDbContext.
        /// </summary>
        /// <param name="context">The database context used to interact with the database.</param>
        public ProductRepository(GlobalDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new product to the storage.
        /// </summary>
        /// <param name="storage">The product to be added.</param>
        public void AddProduct(Products storage)
        {
            // Add the product to the StorageData collection and save changes.
            _context.StorageData.Add(storage);
            _context.SaveChanges();
        }

        /// <summary>
        /// Counts the total number of products in the storage.
        /// </summary>
        /// <returns>The total number of products.</returns>
        public int CountProducts()
        {
            // Returns the count of products in the StorageData collection.
            return _context.StorageData.Count();
        }

        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product to retrieve.</param>
        /// <returns>The product that matches the given id, including related reviews.</returns>
        public Products GetProduct(int id)
        {
            // Retrieve the product including its reviews from the StorageData collection.
            return _context.StorageData
                .Include(y => y.Reviews) // Include related reviews for the product.
                .First(x => x.Id == id); // Find the first matching product by Id.
        }

        /// <summary>
        /// Retrieves all products from the storage.
        /// </summary>
        /// <returns>An IQueryable collection of all products.</returns>
        public IQueryable<Products> GetProducts()
        {
            // Returns all products as an IQueryable to allow further querying.
            return _context.StorageData.AsQueryable();
        }
    }
}
