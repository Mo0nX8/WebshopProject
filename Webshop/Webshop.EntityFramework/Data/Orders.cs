namespace Webshop.EntityFramework.Data
{
    public class Orders
    {
        public int Id { get; set; }
        public int OrderID { get; set; }
        public DateTime DateOfOrder { get; set; }
        public int UserId { get; set; }
        public UserData User { get; set; }
    }
}
