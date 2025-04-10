namespace Webshop.EntityFramework.Data
{
    /// <summary>
    /// Contains specific cart's items. This is a connecting class for Carts and Products. 
    /// </summary>
    public class CartItem
    {
        /// <summary>
        /// Foreign key referencing the shopping cart this item belongs to.
        /// </summary>
        public int CartId { get; set; }
        /// <summary>
        /// Navigation property for the associated shopping cart.
        /// </summary>
        public ShoppingCart Cart { get; set; }
        /// <summary>
        /// Foreign key referencing the product in the cart.
        /// </summary>
        
        public int ProductId { get; set; }
        /// <summary>
        /// Navigation property for the associated product.
        /// </summary>
        public Products Product { get; set; }
        /// <summary>
        /// The quantity of the product in the shopping cart.
        /// </summary>
        public int Quantity { get; set; }
    }
}
