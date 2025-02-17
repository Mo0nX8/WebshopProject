namespace Webshop.EntityFramework.Data
{
    /// <summary>
    /// Contains specific cart's items
    /// </summary>
    public class CartItem
    {
        public int CartId { get; set; }
        public ShoppingCart Cart { get; set; }
        
        public int ProductId { get; set; }
        public Products Product { get; set; }

        public int Quanity { get; set; }
    }
}
