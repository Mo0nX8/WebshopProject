using Microsoft.AspNetCore.Mvc;
using Webshop.Services.Interfaces;

namespace WebshopWeb.Controllers
{
    /// <summary>
    /// Controller for managing the PC builder API endpoints.
    /// Provides functionalities for retrieving all products and filtering products based on compatibility.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PCBuilderController : Controller
    {
        private ICompatibilityService compatibilityService;
        /// <summary>
        /// Initializes a new instance of the PCBuilderController.
        /// </summary>
        /// <param name="compatibilityService">Service for checking product compatibility.</param>
        public PCBuilderController(ICompatibilityService compatibilityService)
        {
            this.compatibilityService = compatibilityService;
        }
        /// <summary>
        /// Gets all available products from the compatibility service.
        /// </summary>
        /// <returns>Returns a JSON result containing all products.</returns>
        [HttpGet("getallproducts")]
        public JsonResult GetAllProducts()
        {
            var products = compatibilityService.GetAllProducts();
            return Json(products.ToList());
        }
        /// <summary>
        /// Filters products based on the selected motherboard, CPU, RAM, and case.
        /// </summary>
        /// <param name="motherboardId">The selected motherboard ID.</param>
        /// <param name="cpuId">The selected CPU ID.</param>
        /// <param name="ramId">The selected RAM ID.</param>
        /// <param name="caseId">The selected case ID.</param>
        /// <returns>Returns a JSON result containing the filtered products based on compatibility.</returns>
        [HttpGet("filterproducts")]
        public JsonResult FilterProducts(int? motherboardId, int? cpuId, int? ramId, int? caseId)
        {
            var products = compatibilityService.FilterProducts(motherboardId, cpuId, ramId, caseId);
            return Json(products.ToList());
        }


    }
}
