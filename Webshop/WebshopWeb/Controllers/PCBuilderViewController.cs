using Microsoft.AspNetCore.Mvc;

namespace WebshopWeb.Controllers
{
    [Route("PCBuilder")]
    public class PCBuilderViewController : Controller
    {
        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
