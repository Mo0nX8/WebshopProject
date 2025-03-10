using Microsoft.AspNetCore.Mvc;

namespace WebshopWeb.Controllers
{
    public class PCBuilderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
