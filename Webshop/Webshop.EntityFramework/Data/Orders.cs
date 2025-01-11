using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.EntityFramework.Data
{
    public class Orders
    {
        public int UserID { get; set; }
        public int OrderID { get; set; }
        public DateTime DateOfOrder { get; set; }
    }
}
