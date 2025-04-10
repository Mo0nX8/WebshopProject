namespace Webshop.EntityFramework.Data
{
    /// <summary>
    /// This class is for Orders
    /// </summary>
    public class Orders
    {
        /// <summary>
        /// Unique identifier for orders.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Date and time when the order was placed.
        /// 
        /// </summary>
        public DateTime DateOfOrder { get; set; }
        /// <summary>
        /// Foreign key referencing the user who placed the order. 
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// The total price of the order.
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// The shipping method chosen for the order.
        /// </summary>
        public string ShippingOption { get; set; }
        /// <summary>
        /// The payment method chosen for the order.
        /// </summary>
        public string PaymentOption { get; set; }
        /// <summary>
        /// Collection of order items associated with the order.
        /// Each item represents a product, its price and its quantity
        /// </summary>
        public ICollection<OrderItem> OrderItems { get; set; }
        /// <summary>
        /// Navigation property for the user who made the order.
        /// This allows access to the user's details. 
        /// </summary>
        public UserData User { get; set; }
    }
}
