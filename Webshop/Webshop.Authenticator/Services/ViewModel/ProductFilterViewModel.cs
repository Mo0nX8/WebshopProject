using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;

namespace Webshop.Services.Services.ViewModel
{
    public class ProductFilterViewModel
    {
        public string? ProductName { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public List<Products> Products { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 30;
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public int TotalItems { get; set; }
        public string SortOrder { get; set; }
    }
}
