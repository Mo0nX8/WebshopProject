using Microsoft.EntityFrameworkCore;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Product;
using Webshop.Services.Interfaces;

namespace Webshop.Services.Services.ProductService
{
    public class ProductService : IProductServices
    {
        private readonly IProductManager productManager;

        public ProductService(IProductManager productManager)
        {
            this.productManager = productManager;
        }

        
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
