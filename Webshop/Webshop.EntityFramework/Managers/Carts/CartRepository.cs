using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Carts
{
    public class CartRepository : ICartRepository
    {
        private readonly GlobalDbContext _context;

        public CartRepository(GlobalDbContext context)
        {
            _context = context;
        }

        public void AddCart(ShoppingCart cart)
        {
            _context.Carts.Add(cart);
            _context.SaveChanges();
        }

        public void AddProductsToCart(int userId, List<int> productIds)
        {
            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new ShoppingCart { UserId = userId };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }

            foreach (var productId in productIds)
            {
                var product = _context.StorageData.Find(productId);
                if (product == null) continue;
                if (product != null)
                {
                    var existingCartItem = _context.CartItems
                        .FirstOrDefault(ci => ci.CartId == cart.Id && ci.ProductId == productId);

                    if (existingCartItem != null)
                    {
                        existingCartItem.Quanity++;
                    }
                    else
                    {
                        _context.CartItems.Add(new CartItem
                        {
                            CartId = cart.Id,
                            ProductId = productId,
                            Quanity = 1
                        });
                    }
                }
            }

            _context.SaveChanges();
        }

        public ShoppingCart? GetCart(int cartId)
        {
            return _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefault(c => c.Id == cartId);
        }

        public List<Products> GetProducts(int cartId)
        {
            var cart = _context.Carts
               .Include(c => c.CartItems)
               .ThenInclude(ci => ci.Product)
               .FirstOrDefault(x => x.Id == cartId);

            return cart?.CartItems?.Select(ci => ci.Product).ToList() ?? new List<Products>();
        }

        public void RemoveItemFromCart(int cartId, int productId)
        {
            var cart = _context.Carts.FirstOrDefault(x => x.Id == cartId);
            var cartItem = _context.CartItems
                          .FirstOrDefault(ci => ci.CartId == cartId && ci.ProductId == productId);
            if (cartItem != null)
            {
                cart.CartItems.Remove(cartItem);
                _context.Carts.Update(cart);
                _context.SaveChanges();
            }
        }
        public void SaveCart(int cartId)
        {
            _context.Carts.Update(_context.Carts.FirstOrDefault(c => c.Id == cartId));
            _context.SaveChanges();
        }
    }
}
