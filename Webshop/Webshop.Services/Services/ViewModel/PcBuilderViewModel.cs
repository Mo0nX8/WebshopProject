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
        public string name { get; set; }
        public string? category { get; set; }
        public string[] tags { get; set; }
    }
}
