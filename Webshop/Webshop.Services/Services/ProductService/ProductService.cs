using Microsoft.EntityFrameworkCore;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Product;
using Webshop.Services.Interfaces;

namespace Webshop.Services.Services.ProductService
{ 
      /// <summary>
      /// This is the implementation for <see cref="IProductServices"/>.
      /// Provides methods for handling products.
      /// </summary>>
    public class ProductService : IProductServices
    {
        private readonly IProductManager productManager;
        /// <summary>
        ///  Constructor for initializing <see cref="ProductService"/> with dependencies.
        /// </summary>
        /// <param name="productManager">Service for product management.</param>
        public ProductService(IProductManager productManager)
        {
            this.productManager = productManager;
        }

        /// <summary>
        /// Retrieve random products from database.
        /// </summary>
        /// <param name="totalItems">Total amount of items.</param>
        /// <returns><see cref="List{Products}"/> of products, Count=15 </returns>
        public List<Products> GetRandomProducts(int totalItems)
        {
            Random random = new Random();
            var skipCount = random.Next(0, totalItems);
            var products = productManager.GetProducts()
                .OrderBy(p => Guid.NewGuid())
                .Where(p => p.Price != 0)
                .Skip(skipCount)
                .Take(15)
                .ToList();
            return products;
        }
        /// <summary>
        /// Applies filtering, searching, sorting, and pagination logic to a product query.
        /// </summary>
        /// <param name="query">The initial product query.</param>
        /// <param name="searchValue">Search term to filter product names and tags (case-insensitive, accent-insensitive).</param>
        /// <param name="minPrice">Optional minimum price filter.</param>
        /// <param name="maxPrice">Optional maximum price filter.</param>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="totalItems">The total number of items after filtering (before pagination).</param>
        /// <param name="sortOrder">
        /// Sorting mode:
        /// <list type="bullet">
        /// <item><description><c>name_asc</c>: Sort by name ascending</description></item>
        /// <item><description><c>name_desc</c>: Sort by name descending</description></item>
        /// <item><description><c>price_asc</c>: Sort by price ascending</description></item>
        /// <item><description><c>price_desc</c>: Sort by price descending</description></item>
        /// </list>
        /// </param>
        /// <returns>A filtered, sorted, and paginated <see cref="IQueryable{Products}"/>.</returns>
        public IQueryable<Products> ApplyFilterAndSearchForPagination(IQueryable<Products> query, string searchValue, int? minPrice, int? maxPrice, int pageNumber, int pageSize, out int totalItems, string sortOrder)
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query
                     .Where(x => EF.Functions.Collate(x.ProductName, "Latin1_General_CI_AI")
                    .Contains(searchValue.ToLower()) || x.Tags.Any(t => EF.Functions.Collate(t, "Latin1_General_CI_AI")
                    .Contains(searchValue.ToLower())));
            }
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }
            if (sortOrder is not null)
            {
                switch (sortOrder)
                {
                    case "name_asc":
                        query = query.OrderBy(p => p.ProductName);
                        break;
                    case "name_desc":
                        query = query.OrderByDescending(p => p.ProductName);
                        break;
                    case "price_asc":
                        query = query.OrderBy(p => p.Price);
                        break;
                    case "price_desc":
                        query = query.OrderByDescending(p => p.Price);
                        break;
                }
            }
            totalItems = query.Count();
            return query;
        }
    }
}
