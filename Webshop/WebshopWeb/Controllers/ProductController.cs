using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webshop.EntityFramework;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Interfaces.Cart;
using Webshop.EntityFramework.Managers.Interfaces.Product;
using Webshop.Services.Services.ViewModel;

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
            var totalItems = productManager.Count();
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
        public IActionResult Details(int id)
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
        public IActionResult Search(string searchValue, int pageNumber = 1, int pageSize = 30)
        {
            IQueryable<Products> productsQuery = productManager.GetProducts();
            if (!string.IsNullOrEmpty(searchValue))
            {
                productsQuery = productsQuery
                     .Where(x => EF.Functions.Collate(x.ProductName, "Latin1_General_CI_AI")
                    .Contains(searchValue.ToLower()) || x.Tags.Any(t => EF.Functions.Collate(t, "Latin1_General_CI_AI")
                    .Contains(searchValue.ToLower())));
            }

            var totalItems = productsQuery.Count();
            var products = productsQuery
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            var model1 = new ProductFilterViewModel
            {
                Products = products,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems

            };
            ViewBag.searchValue = searchValue;
            return View("Index", model1);
        }
        [HttpPost]
        public IActionResult Filter(ProductFilterViewModel filter, int pageNumber=1, int pageSize=30)
        {
            var query = productManager.GetProducts();

            if (!string.IsNullOrEmpty(filter.ProductName))
            {
                query = query.Where(p => p.ProductName.Contains(filter.ProductName));
            }
            if (filter.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= filter.MinPrice.Value);
            }
            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);
            }
            var totalItems = query.Count();


            var filteredProducts = query
                .Skip((pageNumber-1)*pageSize)
                .Take(pageSize)
                .ToList();
            var model = new ProductFilterViewModel
            {
                ProductName = filter.ProductName,
                MinPrice = filter.MinPrice,
                MaxPrice = filter.MaxPrice,
                Products = filteredProducts,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems
            };
            return View("Index", model);
        }
    }
}
    




