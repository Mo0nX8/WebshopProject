using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Buffers;
using Webshop.EntityFramework;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Interfaces.Cart;
using Webshop.EntityFramework.Managers.Interfaces.Product;
using Webshop.Services.Services.ViewModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebshopWeb.Controllers
{
    public class ProductController : Controller
    {
        private GlobalDbContext _context;
        private List<Products> products;
        private IProductManager productManager;
        private ICartManager cartManager;

        public ProductController(GlobalDbContext context, IProductManager productManager, ICartManager cartManager)
        {
            this._context = context;
            this.productManager = productManager;
            this.cartManager = cartManager;
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = 30)
        {
            var totalItems = productManager.CountProducts();
            products = _context.StorageData
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            var model = new ProductFilterViewModel
            {
                Products = products,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems
            };
            return View(model);
        }
        public IActionResult Details(string name, int id)
        {
            var product = productManager.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        public IActionResult AddToCart(int Id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Authentication");
            }
            var cart = cartManager.GetCart(HttpContext.Session.GetInt32("CartId").Value);
            var product = productManager.GetProduct(Id);
            if (product == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var existingItem = _context.CartItems.FirstOrDefault(x => x.ProductId == Id);
            if (existingItem != null)
            {
                existingItem.Quanity++;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = Id,
                    Quanity = 1
                });
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Cart", cart.CartItems.ToList());
        }
        public IActionResult Search(string searchValue, int pageNumber = 1, int pageSize = 30, int? minPrice=null, int? maxPrice=null, string? sortOrder = null)
        {
            IQueryable<Products> productsQuery = productManager.GetProducts();
            int totalItems;
            var filteredProducts=ApplyFilterAndSearchForPagination(productsQuery,searchValue,minPrice,maxPrice,pageNumber,pageSize, out totalItems, sortOrder).ToList();
            var model = new ProductFilterViewModel
            {
                Products = filteredProducts,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                ProductName=searchValue,
                SortOrder=sortOrder

            };
            ViewBag.searchValue = searchValue;
            ViewBag.sortOrder = sortOrder;
            return View("Index", model);
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
            if(sortOrder is not null)
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
            totalItems= query.Count();
            return query.Skip((pageNumber-1)*pageSize).Take(pageSize);
        }
        [HttpPost]
        public IActionResult AddReview(int productId, int rating, string comment)
        {
            if (rating < 1 || rating > 5)
            {
                return BadRequest("Az értékelésnek 1 és 5 között kell lennie.");
            }

            var review = new Review
            {
                ProductId = productId,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.Now
            };

            _context.Reviews.Add(review);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = productId });
        }
        [HttpGet]
        public IActionResult GetSearchSuggestions(string searchValue)
        {
            var suggestions = productManager.GetProducts()
                .Where(x => x.ProductName.Contains(searchValue) || x.Tags.Contains(searchValue))
                .Take(5)
                .Select(p => new { id = p.Id, productName = p.ProductName })
                .ToList();
            return Json(suggestions);
        }
    }
}
    




