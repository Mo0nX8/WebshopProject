using Microsoft.AspNetCore.Mvc;

namespace WebshopWeb.Controllers
{
    /// <summary>
    /// This controller is for handling the PCBuilder tool's view.
    /// </summary>
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
