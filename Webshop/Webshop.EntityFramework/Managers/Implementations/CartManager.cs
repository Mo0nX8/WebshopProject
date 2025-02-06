using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Interfaces.Cart;
using Webshop.EntityFramework.Migrations;

namespace Webshop.EntityFramework.Managers.Implementations
{
   
    public class CartManager : ICartManager
    {
        private GlobalDbContext _context;

        public CartManager(GlobalDbContext context)
        {
            _context = context;
        }

        public void AddCart(ShoppingCart cart)
        {
            _context.Carts.Add(cart);
            _context.SaveChanges();
        }

        public ShoppingCart GetCart(int cartId)
        {
            return _context.Carts.FirstOrDefault(x => x.Id == cartId);
        }

        public List<Products> GetProduct(int cartId)
        {
            var cart = _context.Carts.FirstOrDefault(x => x.Id == cartId);
            if (cart is not null && cart.Products is not null)
            {
                return cart.Products.ToList();
            }
            return new List<Products>();

        }
    }
}
