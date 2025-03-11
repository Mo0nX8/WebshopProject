using Microsoft.AspNetCore.Mvc;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Product;
using Webshop.Services.Interfaces;

namespace WebshopWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PCBuilderController : Controller
    {
        private ICompatibilityService compatibilityService;
        private IProductManager productManager;

        public PCBuilderController(ICompatibilityService compatibilityService, IProductManager productManager)
        {
            this.compatibilityService = compatibilityService;
            this.productManager = productManager;
        }

        [HttpGet("getram")]
        public IActionResult GetCompatibleRams(int motherboardId)
        {
            var query = compatibilityService.GetRamCompatibleWithMotherboard(motherboardId)
                .Select(p=>new
                {
                    id=p.Id,
                    name=p.ProductName
                })
                .ToList();
            return Json(query);
        }
        [HttpGet("GetCpu")]
        public IActionResult GetCompatibleCPUs(int cpuId)
        {
            var query = compatibilityService.GetCPUCompatibleWithMotherboard(cpuId)
                .Select(p => new
                {
                    id = p.Id,
                    name = p.ProductName
                })
                .ToList(); ;
            return Json(query);
        }
        [HttpGet("GetMotherboard")]
        public IActionResult GetCompatibleMotherboards(int caseId)
        {
            var query = compatibilityService.GetMotherboardCompatibleWithCase(caseId);
            return Json(query.ToList());
        }
        [HttpGet("GetAllPCParts")]
        public IActionResult GetAllPCParts()
        {
            var cpus = productManager.GetProducts()
                        .Where(x => x.Tags.Any(t => t.Contains("cpu")))
                        .Select(p => new { p.Id, p.ProductName }).ToList();
            var ram = productManager.GetProducts()
                        .Where(x => x.Tags.Any(t => t.Contains("memória")))
                        .Select(p => new { p.Id, p.ProductName }).ToList();
            var motherboards = productManager.GetProducts()
                        .Where(x => x.Tags.Any(t => t.Contains("alaplap")))
                        .Select(p => new { p.Id, p.ProductName }).ToList();
            var cases = productManager.GetProducts()
                        .Where(x => x.Tags.Any(t => t.Contains("ház")))
                        .Select(p => new { p.Id, p.ProductName }).ToList();

            return Json(new { cpus, ram, motherboards, cases });
        }


    }
}
