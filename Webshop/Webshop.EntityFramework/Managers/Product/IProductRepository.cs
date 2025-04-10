using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Product
{
    /// <summary>
    /// Interface for the ProductRepository, defining methods for managing products.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Adds a new product to the storage.
        /// </summary>
        /// <param name="storage">The product to be added.</param>
        void AddProduct(Products storage);

        /// <summary>
        /// Counts the total number of products in the storage.
        /// </summary>
        /// <returns>The total number of products.</returns>
        int CountProducts();

        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product to retrieve.</param>
        /// <returns>The product that matches the given id.</returns>
        Products GetProduct(int id);

        /// <summary>
        /// Retrieves all products from the storage.
        /// </summary>
        /// <returns>An IQueryable collection of all products.</returns>
        IQueryable<Products> GetProducts();
    }
}
