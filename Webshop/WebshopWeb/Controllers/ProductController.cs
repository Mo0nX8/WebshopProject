using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webshop.EntityFramework;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Interfaces.Product;

namespace WebshopWeb.Controllers
{
    public class ProductController : Controller
    {
        private GlobalDbContext _context;
        private List<Products> products;
        private IProductManager productManager;

        public ProductController(GlobalDbContext context, IProductManager productManager)
        {
            this._context = context;
            this.productManager = productManager;
        }

        public IActionResult Index()
        {
            products = _context.StorageData.ToList();
            return View(products);
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
            var cart = _context.Carts.Include(c => c.CartItems)
                              .ThenInclude(ci => ci.Product)
                              .FirstOrDefault(c => c.UserId == userId.Value);
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
            var products = _context.StorageData
                .Where(x => EF.Functions.Collate(x.ProductName, "Latin1_General_CI_AI")
                            .Contains(searchValue.ToLower()) ||
                            x.Tags.Any(t => EF.Functions.Collate(t, "Latin1_General_CI_AI")
                                          .Contains(searchValue.ToLower())))
                .ToList();

            if (products.Count < 1)
            {
                products = _context.StorageData.ToList();
            }

            return View("Index", products);
        }



    }
}
