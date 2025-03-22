using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webshop.EntityFramework.Data
{
    /// <summary>
    /// Contains user's data
    /// </summary>
    public class UserData
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string? Password { get; set; }
        public string EmailAddress { get; set; }
        public string? GoogleId { get; set; }
        public string? FacebookId { get; set; }
        public string? GitHubId { get; set; }
        public bool IsAdmin { get; set; }
        public string? Code { get; set; }
        public DateTime? ExpirationDate { get; set; }
        

        
        public Address? Address { get; set; }

        public ICollection<Orders> Orders { get; set; }
        public ShoppingCart Cart { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Fogadd el az Általános Szerződési Feltételeket!")]
        public bool Terms { get; set; }
    }
}
