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

        /// <summary>
        /// Initializes a new instance of the CartManager class
        /// </summary>
        /// <param name="cartRepository">The repository used for cart-related database operations.</param>
        public CartManager(ICartRepository cartRepository)
        {
            this.cartRepository = cartRepository;
        }
        /// <summary>
        /// Adds a new shopping cart to the repository.
        /// </summary>
        /// <param name="cart">The shopping cart to add.</param>
        public void AddCart(ShoppingCart cart)
        {
            cartRepository.AddCart(cart);
        }
        /// <summary>
        /// Retrieves a shopping cart by its unique identifier. 
        /// Returns null if the cartId is null or no cart found.
        /// </summary>
        /// <param name="cartId">The unique identifier of the cart.</param>
        /// <returns>A shopping cart object or null, if cart not found.</returns>
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
        /// <summary>
        /// Retrieves all the products associated with the specified shopping cart.
        /// </summary>
        /// <param name="cartId">The unique identifier of the cart.</param>
        /// <returns>A list of products.</returns>
        public List<Products> GetProduct(int cartId)
        {
            return cartRepository.GetProducts(cartId);
        }
        /// <summary>
        /// Removes a specific item form the cart.
        /// </summary>
        /// <param name="cartId">The unique identifier of the cart.</param>
        /// <param name="productId">The unique identifier of the product.</param>
        public void RemoveItemFromCart(int? cartId, int productId)
        {
            if (cartId.HasValue)
            {
                cartRepository.RemoveItemFromCart(cartId.Value, productId);
            }
        }
        /// <summary>
        /// Saves the current state of the cart.
        /// </summary>
        /// <param name="cartId">The unique identifier of the cart to save.</param>
        public void SaveCart(int cartId)
        {
            if(cartId!=null)
            {
                cartRepository.SaveCart(cartId);
            }
        }
    }
}
