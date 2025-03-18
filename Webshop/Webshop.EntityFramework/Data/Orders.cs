using System.ComponentModel.DataAnnotations.Schema;

namespace Webshop.EntityFramework.Data
{
    /// <summary>
    /// This table is for Orders
    /// </summary>
    public class Orders
    {
        public int Id { get; set; }
        public DateTime DateOfOrder { get; set; }
        public int UserId { get; set; }
        public int Price { get; set; }
        public string ShippingOption { get; set; }
        public string PaymentOption { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public UserData User { get; set; }
    }
}
