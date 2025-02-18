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
    }
}
