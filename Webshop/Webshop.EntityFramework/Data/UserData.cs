using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webshop.EntityFramework.Data
{
    /// <summary>
    /// This class sets user's details.
    /// </summary>
    public class UserData
    {
        /// <summary>
        /// Unique identifier for the user. 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The username chosen by the user.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// The password chosen by the user for logging in.
        /// This can be null, when the user logs in through a third-party service.
        /// </summary>
        public string? Password { get; set; }
        /// <summary>
        /// The email address chosen by the user for logging in, recovery, account notifications.
        /// </summary>
        public string EmailAddress { get; set; }
        /// <summary>
        /// The user's Google ID if they logged in via Google authentication.
        /// This field can be null if the user hasn't authenticated using Google.
        /// </summary>
        public string? GoogleId { get; set; }
        /// <summary>
        /// The user's Facebook ID if they logged in via Facebook authentication.
        /// This field can be null if the user hasn't authenticated using Facebook.
        /// </summary>
        public string? FacebookId { get; set; }
        /// <summary>
        /// The user's GitHub ID if they logged in via GitHub authentication.
        /// This field can be null if the user hasn't authenticated using GitHub.
        /// </summary>
        public string? GitHubId { get; set; }
        /// <summary>
        /// Indicates if the user is admin or not.
        /// </summary>
        public bool IsAdmin { get; set; }
        /// <summary>
        /// A verification code sent to the user for purposes like password reset, authetication.
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// The expiration date for the verification code, if applicable.
        /// This is used to limit the validity of the verification code.
        /// </summary>
        public DateTime? ExpirationDate { get; set; }
        /// <summary>
        /// The user's address. This can be null if the user hasn't provided an address.
        /// </summary>
        public Address? Address { get; set; }
        /// <summary>
        /// A collection of orders placed by the user.
        /// </summary>

        public ICollection<Orders> Orders { get; set; }
        /// <summary>
        /// A cart associated with the user.
        /// </summary>
        public ShoppingCart Cart { get; set; }
        /// <summary>
        /// Indicates whether the user accepted or not the Terms and Conditions.
        /// This field is required for account creation.
        /// </summary>

        [NotMapped]
        [Required(ErrorMessage = "Fogadd el az Általános Szerződési Feltételeket!")]
        public bool Terms { get; set; }
    }
}
