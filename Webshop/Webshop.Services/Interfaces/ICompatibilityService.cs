using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;

namespace Webshop.Services.Interfaces
{
    public interface ICompatibilityService
    {
        IQueryable<Products> GetRamCompatibleWithMotherboard(int motherboardId);
        IQueryable<Products> GetCPUCompatibleWithMotherboard(int cpuId);
        IQueryable<Products> GetMotherboardCompatibleWithCase(int caseId);
    }
}
