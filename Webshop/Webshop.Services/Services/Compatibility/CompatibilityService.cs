using Azure;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Product;
using Webshop.Services.Interfaces;
using Webshop.Services.Services.ViewModel;

namespace Webshop.Services.Services.Compatibility
{
    public class CompatibilityService : ICompatibilityService
    {
        private readonly IProductManager productManager;

        public CompatibilityService(IProductManager productManager)
        {
            this.productManager = productManager;
        }


        public IQueryable<PcBuilderViewModel> GetAllProducts()
        {
            var products = productManager.GetProducts()
               .Where(x => x.Tags.Any(y => y.Contains("cpu") 
               || x.Tags.Any(y => y.Contains("alaplap") 
               || x.Tags.Any(y => y.Contains("memória") 
               || x.Tags.Any(y => y.Contains("gépház"))) 
               || x.Tags.Any(tag => tag.Contains("videókártya")) 
               || x.Tags.Any(tag => tag.Contains("tápegység")) 
               || x.Tags.Any(tag => tag.Contains("Processzor_hűtő")) 
               || x.Tags.Any(tag => tag.Contains("SSD")))))

                .Select(p => new PcBuilderViewModel
                {
                    Id = p.Id,
                    Name = p.ProductName,
                    Category = AssignCategory(p),
                    Tags = p.Tags,
                    Price = p.Price
                });

            return products;

        }
        private static string AssignCategory(Products product)
        {
            if (product.Tags.Any(t => t.Contains("CPU")))
            {
                return "Processzor";
            }
            if (product.Tags.Any(t => t.Contains("alaplap")))
            {
                return "Alaplap";
            }
            if (product.Tags.Any(t => t.Contains("memória")))
            {
                return "Memória";
            }
            if (product.Tags.Any(t => t.Contains("Gépház")))
            {
                return "Gépház";
            }
            if (product.Tags.Any(t => t.Contains("videókártya")))
            {
                return "Videókártya";
            }
            if (product.Tags.Any(t => t.Contains("tápegység")))
            {
                return "Tápegység";
            }
            if (product.Tags.Any(t => t.Contains("Processzor_hűtő")))
            {
                return "ProcesszorHűtő";
            }
            if (product.Tags.Any(t => t.Contains("SSD")))
            {
                return "SSD";
            }
            return "Other";
        }

        public IQueryable<PcBuilderViewModel> FilterProducts(int? motherboardId, int? cpuId, int? ramId, int? caseId)
        {
            var products = GetAllProducts();
            var productQuery = products.AsQueryable();

            var relevantSocketTags = new[] { "AM4", "AM5", "Socket 1700", "Socket 1851" };
            var relevantRamTags = new[] { "DDR4", "DDR5" };
            var relevantCaseTags = new[] { "EATX", "ATX", "Micro-ATX", "Mini-ITX" };
            if (motherboardId.HasValue || cpuId.HasValue || ramId.HasValue || caseId.HasValue)
            {
                var motherboardTags = motherboardId.HasValue
                    ? productManager.GetProducts()
                        .Where(x => x.Id == motherboardId)
                        .SelectMany(y => y.Tags)
                        .ToArray()
                    : Array.Empty<string>();

                var cpuTags = cpuId.HasValue
                    ? productManager.GetProducts()
                        .Where(x => x.Id == cpuId)
                        .SelectMany(y => y.Tags)
                        .ToArray()
                    : Array.Empty<string>();
                var ramTags = ramId.HasValue
                    ? productManager.GetProducts()
                        .Where(x => x.Id == ramId)
                        .SelectMany(y => y.Tags)
                        .ToArray()
                    : Array.Empty<string>();
                var caseTags = caseId.HasValue
                   ? productManager.GetProducts()
                       .Where(x => x.Id == caseId)
                       .SelectMany(y => y.Tags)
                       .ToArray()
                   : Array.Empty<string>();

                var combinedTags = motherboardTags.Concat(cpuTags).Concat(ramTags).Concat(caseTags).Distinct().ToArray();

                productQuery = productQuery.Where(p =>
                    p.Tags.Any(tag => combinedTags.Contains(tag) && relevantSocketTags.Contains(tag)) ||
                    p.Tags.Any(tag => combinedTags.Contains(tag) && relevantRamTags.Contains(tag)) ||
                    p.Tags.Any(tag => combinedTags.Contains(tag) && relevantCaseTags.Contains(tag)) ||
                    p.Tags.Any(tag => tag.Contains("videókártya")) ||
                    p.Tags.Any(tag => tag.Contains("SSD")) ||
                    p.Tags.Any(tag => tag.Contains("Processzor_hűtő")) ||
                    p.Tags.Any(tag => tag.Contains("tápegység")));
            }

            return productQuery;
        }
    }
}
