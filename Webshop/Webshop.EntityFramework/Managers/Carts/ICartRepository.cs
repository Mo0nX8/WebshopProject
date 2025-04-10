using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Carts
{
    /// <summary>
    /// Interface for interacting with the shopping cart repository.
    /// Defines the contract for managing shopping carts and related operations.
    /// </summary>
    public interface ICartRepository
    {
        /// <summary>
        /// Adds a new shopping cart to the repository.
        /// </summary>
        /// <param name="cart">The shopping cart to add.</param>
        void AddCart(ShoppingCart cart);

        /// <summary>
        /// Retrieves a shopping cart by its unique identifier.
        /// </summary>
        /// <param name="cartId">The unique identifier of the cart to retrieve.</param>
        /// <returns>The shopping cart if found; otherwise, null.</returns>
        ShoppingCart? GetCart(int cartId);

        /// <summary>
        /// Adds a list of products to the user's shopping cart.
        /// If the cart doesn't exist, a new one is created for the user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to whom the cart belongs.</param>
        /// <param name="productIds">A list of product IDs to add to the cart.</param>
        void AddProductsToCart(int userId, List<int> productIds);

        /// <summary>
        /// Retrieves all products associated with a specified shopping cart.
        /// </summary>
        /// <param name="cartId">The unique identifier of the cart whose products should be retrieved.</param>
        /// <returns>A list of products in the specified cart. An empty list is returned if no products are found.</returns>
        List<Products> GetProducts(int cartId);

        /// <summary>
        /// Removes a specific product from a shopping cart.
        /// </summary>
        /// <param name="cartId">The unique identifier of the cart from which the product should be removed.</param>
        /// <param name="productId">The unique identifier of the product to remove from the cart.</param>
        void RemoveItemFromCart(int cartId, int productId);

        /// <summary>
        /// Saves the current state of the shopping cart to the repository.
        /// </summary>
        /// <param name="cartId">The unique identifier of the cart to save.</param>
        void SaveCart(int cartId);
    }
}
