using Microsoft.EntityFrameworkCore;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Interfaces.Cart;

namespace Webshop.EntityFramework.Managers.Implementations
{
    /// <summary>
    /// This is the implementation for ICartManager.
    /// </summary>

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

        public ShoppingCart? GetCart(int? cartId)
        {
            return _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)  
                .FirstOrDefault(c => c.Id == cartId);
        }
        /// <summary>
        /// This method requires an userId and an integer list as parameters. This method checks if cart not null and add products to the user's cart. 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="productIds"></param>
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

        public List<Products> GetProduct(int cartId)
        {
            var cart = _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product) 
                .FirstOrDefault(x => x.Id == cartId);

            return cart?.CartItems?.Select(ci => ci.Product).ToList() ?? new List<Products>();
        }

    }
}
