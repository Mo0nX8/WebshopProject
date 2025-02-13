using Microsoft.AspNetCore.Mvc;
using Webshop.EntityFramework;
using Webshop.EntityFramework.Data;

namespace WebshopWeb.Controllers
{
    public class ProductController : Controller
    {
        private GlobalDbContext _context;
        private List<Products> products;

        public ProductController(GlobalDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            products = _context.StorageData.ToList();
            return View(products);
        }
    }
}
