namespace Webshop.EntityFramework.Data
{
    /// <summary>
    /// This class represents an address details of an user. 
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Unique identifier for the address.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Foreign key referencing the user this address belongs to.
        /// Nullable, the user can exist without an address.
        /// </summary>
        public int? UserId { get; set; }
        /// <summary>
        /// Represents the user's full name.
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// The city where the address located.
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Postal code of the address.
        /// </summary>
        public string ZipCode { get; set; }
        /// <summary>
        /// Street and number of the address.
        /// </summary>
        public string StreetAndNumber { get; set; }
        /// <summary>
        /// Nacigation property for the associated user.
        /// </summary>
        public UserData User { get; set; }
    }
}
