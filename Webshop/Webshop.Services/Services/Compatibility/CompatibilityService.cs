using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Product;
using Webshop.Services.Interfaces;

namespace Webshop.Services.Services.Compatibility
{
    public class CompatibilityService : ICompatibilityService
    {
        private readonly IProductManager productManager;

        public CompatibilityService(IProductManager productManager)
        {
            this.productManager = productManager;
        }

        public IQueryable<Products> GetCPUCompatibleWithMotherboard(int cpuId)
        {
            var query = productManager.GetProducts();
            var cpuTags = query
                .Where(x => x.Id == cpuId)
                .Select(y => y.Tags)
                .FirstOrDefault();
            IQueryable<Products> compatibleQuery = query;
            foreach (var tag in cpuTags)
            {
                switch (tag)
                {
                    case "AM4":
                        compatibleQuery = compatibleQuery.Where(p => p.Tags.Any(y => y.Contains("alaplap") && p.Tags.Any(y => y.Contains("AM4"))));
                        break;
                    case "AM5":
                        compatibleQuery = compatibleQuery.Where(p => p.Tags.Any(y => y.Contains("alaplap") && p.Tags.Any(y => y.Contains("AM5"))));
                        break;
                    case "LGA1700":
                        compatibleQuery = compatibleQuery.Where(p => p.Tags.Any(y => y.Contains("alaplap") && p.Tags.Any(y => y.Contains("Socket 1700"))));
                        break;
                    case "LGA1851":
                        compatibleQuery = compatibleQuery.Where(p => p.Tags.Any(y => y.Contains("alaplap") && p.Tags.Any(y => y.Contains("Socket 1851"))));
                        break;
                }
            }
            return compatibleQuery;

        }

        public IQueryable<Products> GetMotherboardCompatibleWithCase(int caseId)
        {


            var query = productManager.GetProducts();
            var caseTags = query
                .Where(x => x.Id == caseId)
                .Select(y => y.Tags)
                .FirstOrDefault();
            IQueryable<Products> compatibleQuery = query.Where(p => false);

            foreach (var tag in caseTags)
            {
                
                switch (tag)
                {
                    case "EATX":
                        compatibleQuery = compatibleQuery.Concat(query.Where(p => p.Tags.Contains("alaplap") && p.Tags.Contains("EATX")));

                        break;
                    case "ATX":
                        compatibleQuery = compatibleQuery.Concat(query.Where(p => p.Tags.Contains("alaplap") && p.Tags.Contains("ATX")));

                        break;
                    case "Micro-ATX":
                        compatibleQuery = compatibleQuery.Concat(query.Where(p => p.Tags.Contains("alaplap") && p.Tags.Contains("Micro-ATX")));

                        break;
                    case "Mini-ITX":
                        compatibleQuery = compatibleQuery.Concat(query.Where(p => p.Tags.Contains("alaplap") && p.Tags.Contains("Mini-ITX")));

                        break;
                    default:
                        break;
                }   
            }

            return compatibleQuery;
        }



        public IQueryable<Products> GetRamCompatibleWithMotherboard(int motherboardId)
        {
            var query = productManager.GetProducts();
            var motherboardTags = query
                .Where(x => x.Id == motherboardId)
                .Select(y => y.Tags)
                .FirstOrDefault();
            IQueryable<Products> compatibleQuery = query.Where(p=>false);
            foreach (var tag in motherboardTags)
            {
                switch (tag)
                {
                    case "AM4":
                        compatibleQuery = compatibleQuery.Concat(query.Where(p => p.Tags.Any(y => y.Contains("memória") && p.Tags.Any(x => x.Contains("ddr4")))));
                        break;
                    case "AM5":
                        compatibleQuery = compatibleQuery.Concat(query.Where(p => p.Tags.Any(y => y.Contains("memória") && p.Tags.Any(x => x.Contains("ddr5")))));
                        break;
                    case "socket 1700":
                        compatibleQuery = compatibleQuery.Concat(query.Where(p => p.Tags.Any(y => y.Contains("memória") && p.Tags.Any(x => x.Contains("ddr5")))));
                        break;
                    case "Socket 1851":
                        compatibleQuery = compatibleQuery.Concat(query.Where(p => p.Tags.Any(y => y.Contains("memória") && p.Tags.Any(x => x.Contains("ddr5")))));
                        break;
                }
            }
            return compatibleQuery;
        }


    }
}
