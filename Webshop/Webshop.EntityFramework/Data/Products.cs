namespace Webshop.EntityFramework.Data
{
    public class Products
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Quanity { get; set; }
        public int Price { get; set; }
        public string[] Tags { get; set; }
        public string[] Description { get; set; }


        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
