namespace Webshop.EntityFramework.Data
{
    public class Products
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int ProductCount { get; set; }
        public string[] Tags { get; set; }
    }
}
