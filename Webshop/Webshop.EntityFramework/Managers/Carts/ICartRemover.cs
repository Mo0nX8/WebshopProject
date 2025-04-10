namespace Webshop.EntityFramework.Managers.Carts
{
    /// <summary>
    /// Interface for removing items from a shopping cart.
    /// </summary>
    public interface ICartRemover
    {
         /// <summary>
         /// Removes a specific product from the shopping cart.
         /// </summary>
         /// <param name="cartId">The unique identifier of the cart from which the product should be removed.</param>
         /// <param name="productId">The ID of the product to be removed from the cart.</param>
        void RemoveItemFromCart(int? cartId, int productId);
    }
}
