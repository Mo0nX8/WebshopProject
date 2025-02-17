using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.EntityFramework.Data
{
    /// <summary>
    /// This is the shopping cart model
    /// </summary>
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserData User { get; set; }
        public ICollection<CartItem>? CartItems { get; set; }=new List<CartItem>();


    }
}
