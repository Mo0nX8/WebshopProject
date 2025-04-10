using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Carts
{
    /// <summary>
    /// This is a CartManager
    /// </summary>
    public interface ICartManager : ICartRemover, IGetCart
    {
        /// <summary>
        /// This method adds a cart to the database. It requires a Cart parameter.
        /// </summary>
        /// <param name="cart">The unique identifier of the cart.</param>
        void AddCart(ShoppingCart cart);
        /// <summary>
        /// This method save a cart's current state.
        /// </summary>
        /// <param name="cartId">The unique identifier of the cart.</param>
        void SaveCart(int cartId);
    }
}
