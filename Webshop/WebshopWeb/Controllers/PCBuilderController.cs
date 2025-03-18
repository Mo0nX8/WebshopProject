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
        [HttpGet("getallproducts")]
        public JsonResult GetAllProducts()
        {
            var products = compatibilityService.GetAllProducts();
            return Json(products.ToList());
        }
        [HttpGet("filterproducts")]
        public JsonResult FilterProducts(int? motherboardId, int? cpuId, int? ramId, int? caseId)
        {
            var products = compatibilityService.FilterProducts(motherboardId, cpuId, ramId, caseId);
            return Json(products.ToList());
        }


    }
}
