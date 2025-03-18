using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;
using Webshop.Services.Services.ViewModel;

namespace Webshop.Services.Interfaces
{
    public interface ICompatibilityService
    {
        IQueryable<PcBuilderViewModel> GetAllProducts();
        IQueryable<PcBuilderViewModel> FilterProducts(int? motherboardId, int? cpuId, int? ramId, int? caseId);
    }
}
