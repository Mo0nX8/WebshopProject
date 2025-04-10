
using Microsoft.AspNetCore.Mvc;
using Webshop.EntityFramework;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Carts;
using Webshop.EntityFramework.Managers.Product;
using Webshop.EntityFramework.Managers.Reviews;
using Webshop.Services.Interfaces;
using Webshop.Services.Services.ViewModel;

namespace WebshopWeb.Controllers
{
    /// <summary>
    /// Controller for handling product-related actions such as listing, searching, viewing details, adding to cart, and submitting reviews.
    /// </summary>
    public class ProductController : Controller
    {
        private GlobalDbContext _context;
        private IProductManager productManager;
        private ICartManager cartManager;
        private IProductServices productServices;
        private IReviewManager reviewManager;
        /// <summary>
        /// Initializes a new instance of the ProductController.
        /// </summary>
        /// <param name="context">Service for database.</param>
        /// <param name="productManager">Service for product management.</param>
        /// <param name="cartManager">Service for cart management.</param>
        /// <param name="productServices">Service for products.</param>
        /// <param name="reviewManager">Service for review management.</param>
        public ProductController(GlobalDbContext context, IProductManager productManager, ICartManager cartManager, IProductServices productServices, IReviewManager reviewManager)
        {
            this._context = context;
            this.productManager = productManager;
            this.cartManager = cartManager;
            this.productServices = productServices;
            this.reviewManager = reviewManager;
        }
        /// <summary>
        /// Displays the list of products, with pagination support.
        /// </summary>
        /// <param name="pageNumber">The current page number for pagination (defaults to 1).</param>
        /// <param name="pageSize">The number of products to display per page (defaults to 30).</param>
        /// <returns>A view with a list of products and pagination information.</returns>
        public IActionResult Index(int pageNumber = 1, int pageSize = 30)
        {
            var totalItems = productManager.CountProducts();
            var products = _context.StorageData
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
        /// <summary>
        /// Displays the details of a specific product.
        /// </summary>
        /// <param name="name">The name of the product (used for routing, not directly in the logic).</param>
        /// <param name="id">The ID of the product whose details should be displayed.</param>
        /// <returns>A view showing detailed information about the selected product.</returns>
        public IActionResult Details(string name, int id)
        {
            var product = productManager.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        /// <summary>
        /// Adds one or more selected products to the user's cart.
        /// </summary>
        /// <param name="ids">Comma-separated product IDs to be added.</param>
        /// <returns>
        public IActionResult AddToCart(string ids)
        {
            if(ids is null)
            {
                return Json(new { success = false, message = "Válassz ki terméket!" });
            }
            var productIds = ids.Split(',').Select(int.Parse).ToList();
            
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return Json(new { success = false, message = "Kérlek jelentkezz be!" });
            }
            var cartId = HttpContext.Session.GetInt32("CartId").Value;
            var cart = cartManager.GetCart(cartId);
            if (cart == null)
            {
                return Json(new { success = false, message = "Kosár nem található!" });
            }

            foreach (var id in productIds)
            {
                var product = productManager.GetProduct(id);
                if (product == null)
                {
                    return Json(new { success = false, message = "Nincs ilyen termék!" });
                }

                var existingItem = _context.CartItems.FirstOrDefault(x => x.ProductId == id);
                if (existingItem != null)
                {
                    existingItem.Quantity++;
                }
                else
                {
                    cart.CartItems.Add(new CartItem
                    {
                        ProductId = id,
                        Quantity = 1
                    });
                }
                _context.SaveChanges();
            }
            var msg = "Termék sikeresen hozzáadva a kosárhoz!";
            if(productIds.Count > 1)
            {
                msg = "Termékek sikeresen hozzáadva a kosárhoz!";
            }
            return Json(new { success = true, message = msg });
        }
        /// <summary>
        /// Handles the search functionality, including filtering products by name, price, and sorting order.
        /// </summary>
        /// <param name="searchValue">The search term entered by the user.</param>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The number of products to display per page.</param>
        /// <param name="minPrice">The minimum price filter.</param>
        /// <param name="maxPrice">The maximum price filter.</param>
        /// <param name="sortOrder">The sorting order for the products.</param>
        /// <returns>Returns a view displaying the filtered products.</returns>
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
        /// <summary>
        /// Applies filters and pagination to the product query.
        /// </summary>
        /// <param name="query">The IQueryable products query to apply filters on.</param>
        /// <param name="searchValue">The search term.</param>
        /// <param name="minPrice">The minimum price filter.</param>
        /// <param name="maxPrice">The maximum price filter.</param>
        /// <param name="pageNumber">The current page number for pagination.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="totalItems">The total number of items after applying filters.</param>
        /// <param name="sortOrder">The sorting order.</param>
        /// <returns>Returns the filtered and paginated IQueryable of products.</returns>
        public IQueryable<Products> ApplyFilterAndSearchForPagination(IQueryable<Products> query, string searchValue, int? minPrice, int? maxPrice, int pageNumber, int pageSize, out int totalItems, string sortOrder)
        {
            query=productServices.ApplyFilterAndSearchForPagination(query,searchValue,minPrice,maxPrice,pageNumber,pageSize, out totalItems, sortOrder); 
            return query.Skip((pageNumber-1)*pageSize).Take(pageSize);
        }
        /// <summary>
        /// Adds a review to a product.
        /// </summary>
        /// <param name="productId">The ID of the product being reviewed.</param>
        /// <param name="rating">The rating for the product (1 to 5).</param>
        /// <param name="comment">The review comment.</param>
        /// <returns>Redirects to the product details page.</returns>
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
            
           
            reviewManager.AddReview(review);
            return RedirectToAction("Details", new { id = productId });
        }
        /// <summary>
        /// Provides search suggestions based on the search value.
        /// </summary>
        /// <param name="searchValue">The search term entered by the user.</param>
        /// <returns>Returns a list of product suggestions in JSON format.</returns>
        [HttpGet]
        public IActionResult GetSearchSuggestions(string searchValue)
        {
            var suggestions = productManager.GetProducts()
                .Where(x => x.ProductName.Contains(searchValue) || x.Tags.Any(x=>x.Contains(searchValue)))
                .Take(5)
                .Select(p => new { id = p.Id, productName = p.ProductName, base64Image=p.ImageData !=null ? $"data:image/jpeg;base64,{Convert.ToBase64String(p.ImageData)}" : null })
                .ToList();
            return Json(suggestions);
        }
    }
}
    




