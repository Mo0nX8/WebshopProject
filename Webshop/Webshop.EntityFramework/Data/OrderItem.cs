namespace Webshop.EntityFramework.Data
{
    /// <summary>
    /// This is a connecting class between Order class and products class. 
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// Unique identifier for the item.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Foreign key referencing the order this item belongs to. 
        /// </summary>
        public int OrderId { get; set; } 
        /// <summary>
        /// Foreign key referencing the product that is part of this order.
        /// </summary>
        public int ProductId { get; set; } 
        /// <summary>
        /// The quantity of the product ordered.
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// The price of the product ordered.
        /// </summary>
        public decimal Price { get; set; } 
        /// <summary>
        /// Navigation property for the associated order.
        /// This allows access to the order which contains this item.
        /// </summary>

        public Orders Order { get; set; }
        /// <summary>
        /// Navigation property for the associated product.
        /// This allows access to the product details for this order item.
        /// </summary>
        public Products Product { get; set; }
    }

}
