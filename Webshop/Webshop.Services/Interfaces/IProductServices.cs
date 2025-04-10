using Webshop.EntityFramework.Data;

namespace Webshop.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for product-related services.
    /// </summary>
    public interface IProductServices
    {
        /// <summary>
        /// Retrieves a list of random products.
        /// </summary>
        /// <param name="totalItems">The total number of random products to retrieve.</param>
        /// <returns>A list of random products.</returns>
        List<Products> GetRandomProducts(int totalItems);

        /// <summary>
        /// Applies filters and search criteria to a query and handles pagination.
        /// </summary>
        /// <param name="query">The base query of products to which filters and search will be applied.</param>
        /// <param name="searchValue">The search term to filter products by.</param>
        /// <param name="minPrice">The minimum price for filtering products.</param>
        /// <param name="maxPrice">The maximum price for filtering products.</param>
        /// <param name="pageNumber">The current page number for pagination.</param>
        /// <param name="pageSize">The number of products per page for pagination.</param>
        /// <param name="totalItems">The total number of items matching the filter (out parameter).</param>
        /// <param name="sortOrder">The order in which to sort the products (e.g., ascending or descending).</param>
        /// <returns>A queryable collection of products that match the filter and search criteria, including pagination.</returns>
        IQueryable<Products> ApplyFilterAndSearchForPagination(IQueryable<Products> query, string searchValue, int? minPrice, int? maxPrice, int pageNumber, int pageSize, out int totalItems, string sortOrder);
    }
}
