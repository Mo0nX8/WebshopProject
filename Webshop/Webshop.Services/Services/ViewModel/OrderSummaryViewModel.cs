namespace Webshop.Services.Services.ViewModel
{
    public class OrderSummaryViewModel
    {
        public List<OrderProductViewModel> Products { get; set; }
        public string CustomerName { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingOption { get; set; }
        public string PaymentOption { get; set; }
        public decimal TotalPrice { get; set; }
    }
    public class OrderProductViewModel
    {
        public string Name { get; set; }
        public byte[] ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
