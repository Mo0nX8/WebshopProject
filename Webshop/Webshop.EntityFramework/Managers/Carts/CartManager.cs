using Microsoft.EntityFrameworkCore;
using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Carts
{
    /// <summary>
    /// This is the implementation for ICartManager.
    /// </summary>

    public class CartManager : ICartManager
    {
        private ICartRepository cartRepository;

        public CartManager(ICartRepository cartRepository)
        {
            this.cartRepository = cartRepository;
        }
        public void AddCart(ShoppingCart cart)
        {
            cartRepository.AddCart(cart);
        }

        public ShoppingCart? GetCart(int? cartId)
        {
            if (cartId == null) return null;
            return cartRepository.GetCart(cartId.Value);
        }
        /// <summary>
        /// This method requires an userId and an integer list as parameters. This method checks if cart not null and add products to the user's cart. 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="productIds"></param>
        public void AddProductsToCart(int userId, List<int> productIds)
        {
            cartRepository.AddProductsToCart(userId, productIds);
        }

        public List<Products> GetProduct(int cartId)
        {
            return cartRepository.GetProducts(cartId);
        }

        public void RemoveItemFromCart(int? cartId, int productId)
        {
            if (cartId.HasValue)
            {
                cartRepository.RemoveItemFromCart(cartId.Value, productId);
            }
        }
    }
}
