using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.EntityFramework.Data
{
    public class Cart
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public int UserId { get; set; }
        public UserData User { get; set; }
        public ICollection<Products> Products { get; set; }


    }
}
