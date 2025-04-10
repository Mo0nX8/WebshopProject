namespace Webshop.EntityFramework.Data
{
    /// <summary>
    /// This class represents a cart.
    /// </summary>
    public class ShoppingCart
    {
        /// <summary>
        /// Unique identifier for a cart.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Foreign key referencing the user who owns this cart.
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Navigation property for the user who owns this cart.
        /// This allows acces for the user's details.
        /// </summary>
        public UserData User { get; set; }
        /// <summary>
        /// A collection of items in the shopping cart.
        /// Each item represents a product and its quantity in the cart.
        /// </summary>
        public ICollection<CartItem>? CartItems { get; set; }=new List<CartItem>();


    }
}
