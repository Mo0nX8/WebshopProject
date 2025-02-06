namespace Webshop.EntityFramework.Data
{
    public class UserData
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public bool IsAdmin { get; set; }
        public ICollection<Orders> Orders { get; set; }
        public ShoppingCart Cart { get; set; }





    }
}
