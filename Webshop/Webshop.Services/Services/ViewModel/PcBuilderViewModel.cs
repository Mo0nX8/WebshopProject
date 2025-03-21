using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Services.Services.ViewModel
{
    public class PcBuilderViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Category { get; set; }
        public string[] Tags { get; set; }
        public decimal Price { get; set; }
    }
}
