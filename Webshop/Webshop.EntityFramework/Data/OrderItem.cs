using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.EntityFramework.Data
{
    /// <summary>
    /// This table helps storing Order data
    /// </summary>
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; } 
        public int ProductId { get; set; } 
        public int Quantity { get; set; }
        public decimal Price { get; set; } 

        public Orders Order { get; set; }
        public Products Product { get; set; }
    }

}
