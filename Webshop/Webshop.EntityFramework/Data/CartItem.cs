namespace Webshop.EntityFramework.Data
{
    public class CartItem
    {
        public int CartId { get; set; }
        public ShoppingCart Cart { get; set; }
        
        public int ProductId { get; set; }
        public Products Product { get; set; }

        public int Quanity { get; set; }
    }
}
