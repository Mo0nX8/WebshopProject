using Microsoft.AspNetCore.Mvc;
using Webshop.EntityFramework.Data;
using Webshop.Services.Interfaces;

namespace WebshopWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PCBuilderController : Controller
    {
        private ICompatibilityService compatibilityService;

        public PCBuilderController(ICompatibilityService compatibilityService)
        {
            this.compatibilityService = compatibilityService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("getram")]
        public IActionResult GetCompatibleRams(int motherboardId)
        {
            var query = compatibilityService.GetRamCompatibleWithMotherboard(motherboardId);
            return Json(query.ToList());
        }
        [HttpGet("GetCpu")]
        public IActionResult GetCompatibleCPUs(int cpuId)
        {
            var query = compatibilityService.GetCPUCompatibleWithMotherboard(cpuId);
            return Json(query.ToList());
        }
        [HttpGet("GetMotherboard")]
        public IActionResult GetCompatibleMotherboards(int caseId)
        {
            var query = compatibilityService.GetMotherboardCompatibleWithCase(caseId);
            return Json(query.ToList());
        }
    }
}
