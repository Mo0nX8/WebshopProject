using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;

namespace Webshop.Services.Interfaces
{
    public interface IProductServices
    {
        List<Products> GetRandomProducts(int totalItems);
        IQueryable<Products> ApplyFilterAndSearchForPagination(IQueryable<Products> query, string searchValue, int? minPrice, int? maxPrice, int pageNumber, int pageSize, out int totalItems, string sortOrder);
    }
}
