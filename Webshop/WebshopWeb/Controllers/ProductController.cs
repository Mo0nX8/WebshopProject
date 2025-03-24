
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
    public class ProductController : Controller
    {
        private GlobalDbContext _context;
        private IProductManager productManager;
        private ICartManager cartManager;
        private IProductServices productServices;
        private IReviewManager reviewManager;

        public ProductController(GlobalDbContext context, IProductManager productManager, ICartManager cartManager, IProductServices productServices, IReviewManager reviewManager)
        {
            this._context = context;
            this.productManager = productManager;
            this.cartManager = cartManager;
            this.productServices = productServices;
            this.reviewManager = reviewManager;
        }
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
            query=productServices.ApplyFilterAndSearchForPagination(query,searchValue,minPrice,maxPrice,pageNumber,pageSize, out totalItems, sortOrder); 
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
            
           
            reviewManager.AddReview(review);
            return RedirectToAction("Details", new { id = productId });
        }
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
    




