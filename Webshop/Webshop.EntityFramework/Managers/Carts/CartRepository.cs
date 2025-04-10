using Microsoft.EntityFrameworkCore;
using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Carts
{
    public class CartRepository : ICartRepository
    {
        private readonly GlobalDbContext _context;
        /// <summary>
        /// Initializes a new instance of the CartRepository class.
        /// </summary>
        /// <param name="context">The database context used to interact with the database.</param>
        public CartRepository(GlobalDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Adds a new shopping cart to the database
        /// </summary>
        /// <param name="cart">The shopping cart to be added.</param>
        public void AddCart(ShoppingCart cart)
        {
            _context.Carts.Add(cart);
            _context.SaveChanges();
        }
        /// <summary>
        /// Adds a list of products to the user's shopping cart.
        /// If the cart doesn't exist, it creates a new one for the user.
        /// For each product, it either increments the quantity in the cart or adds a new cart item if the product is not already in the cart.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whom the cart belongs.</param>
        /// <param name="productIds">A list of product Ids to be added to the cart.</param>
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
                        existingCartItem.Quantity++;
                    }
                    else
                    {
                        _context.CartItems.Add(new CartItem
                        {
                            CartId = cart.Id,
                            ProductId = productId,
                            Quantity = 1
                        });
                    }
                }
            }

            _context.SaveChanges();
        }
        /// <summary>
        /// Retrieves the shopping cart by its unique identifier including all associated products.
        /// </summary>
        /// <param name="cartId">The unique identifier of the cart to retrieve.</param>
        /// <returns>A shopping cart object.</returns>
        public ShoppingCart? GetCart(int cartId)
        {
            return _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefault(c => c.Id == cartId);
        }
        /// <summary>
        /// Retrieve all products associated with the given cart ID.
        /// </summary>
        /// <param name="cartId">The unique identifier of the cart.</param>
        /// <returns>A list of products of the cart.</returns>
        public List<Products> GetProducts(int cartId)
        {
            var cart = _context.Carts
               .Include(c => c.CartItems)
               .ThenInclude(ci => ci.Product)
               .FirstOrDefault(x => x.Id == cartId);

            return cart?.CartItems?.Select(ci => ci.Product).ToList() ?? new List<Products>();
        }
        /// <summary>
        /// Removes a specific product from the shopping cart.
        /// </summary>
        /// <param name="cartId">The ID of the cart from which the product should be removed.</param>
        /// <param name="productId">The ID of the product to remove from the cart.</param>

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
        /// <summary>
        /// Saves the current state of the shopping cart to the database.
        /// </summary>
        /// <param name="cartId">The unique identifier of the cart to save.</param>
        public void SaveCart(int cartId)
        {
            _context.Carts.Update(_context.Carts.FirstOrDefault(c => c.Id == cartId));
            _context.SaveChanges();
        }
    }
}
