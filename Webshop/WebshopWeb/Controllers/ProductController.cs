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

        public IActionResult Index()
        {
            products = _context.StorageData.ToList();
            var model = new ProductFilterViewModel
            {
                Products = products
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
            var product=productManager.GetProduct(Id);
            if (product == null) 
            {
                return RedirectToAction("Index", "Home");
            }
            var existingItem=_context.CartItems.FirstOrDefault(x=>x.ProductId == Id);
            if(existingItem!=null)
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
            return RedirectToAction("Index", "Cart",cart.CartItems.ToList());
        }
        public IActionResult Search(string searchValue)
        {
            if(searchValue is null)
            {
                return View("Index", _context.StorageData.ToList());
            }
            var products = productManager.GetProducts()
                .Where(x => EF.Functions.Collate(x.ProductName, "Latin1_General_CI_AI")
                .Contains(searchValue.ToLower()) || x.Tags.Any(t => EF.Functions.Collate(t, "Latin1_General_CI_AI")
                .Contains(searchValue.ToLower())))
                .ToList();


            if (products.Count < 1)
            {
                products = productManager.GetProducts().ToList();

            }
            var model = new ProductFilterViewModel
            {
                Products = products
            };

            return View("Index", model);
        }
        [HttpPost]
        public IActionResult Filter(ProductFilterViewModel filter)
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


            var filteredProducts = query.ToList();
            if (!filteredProducts.Any()) 
            {

            }
            var model = new ProductFilterViewModel
            {
                ProductName = filter.ProductName,
                MinPrice = filter.MinPrice,
                MaxPrice = filter.MaxPrice,
                Products = filteredProducts
            };
            return View("Index",model);
        }



    }
}
