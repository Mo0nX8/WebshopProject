using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Interfaces.Cart;

namespace Webshop.EntityFramework.Managers.Implementations
{
   
    public class CartManager : ICartManager
    {
        private GlobalDbContext _context;

        public CartManager(GlobalDbContext context)
        {
            _context = context;
        }

        public Cart GetProduct(int cartId)
        {
            return _context.Carts.FirstOrDefault(x=>x.Id==cartId);
        }

    }
}
