using Microsoft.AspNetCore.Mvc;
using Webshop.EntityFramework;

namespace WebshopWeb.Controllers
{
    public class ProductController : Controller
    {
        private GlobalDbContext _context;

        public ProductController(GlobalDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.StorageData.ToList());
        }
    }
}
