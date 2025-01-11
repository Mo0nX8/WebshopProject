using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.EntityFramework.Data
{
    public class Order
    {
        public DateTime DateOfOrder { get; set; }
        public int OrderId { get; set; }
    }
}
