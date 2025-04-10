using Webshop.Services.Services.ViewModel;

namespace Webshop.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for services related to product compatibility, specifically for PC building.
    /// </summary>
    public interface ICompatibilityService
    {
        /// <summary>
        /// Retrieves all available products for PC building.
        /// </summary>
        /// <returns>An IQueryable collection of PcBuilderViewModel objects representing all products.</returns>
        IQueryable<PcBuilderViewModel> GetAllProducts();

        /// <summary>
        /// Filters products based on the provided criteria (motherboard, CPU, RAM, and case).
        /// </summary>
        /// <param name="motherboardId">Optional filter for motherboard ID.</param>
        /// <param name="cpuId">Optional filter for CPU ID.</param>
        /// <param name="ramId">Optional filter for RAM ID.</param>
        /// <param name="caseId">Optional filter for case ID.</param>
        /// <returns>An IQueryable collection of filtered PcBuilderViewModel objects.</returns>
        IQueryable<PcBuilderViewModel> FilterProducts(int? motherboardId, int? cpuId, int? ramId, int? caseId);
    }
}
