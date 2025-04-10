using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webshop.EntityFramework.Data
{
    //This class contains everything for Products
    public class Products
    {
        /// <summary>
        /// Unique identifier for a product.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The name of the product.
        /// The product name is stored as string which is max 1000 characters long. 
        /// </summary>
        [Column(TypeName = "nvarchar(1000)")]
        public string ProductName { get; set; }
        /// <summary>
        /// The available Quantity for the product.
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// The price of the product.
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// A list of tags associated with the product.
        /// The tags are stored as an array of strings, which length is set to a maximum of 1000 characters. 
        /// </summary>
        [Column(TypeName = "nvarchar(1000)")]
        public string[] Tags { get; set; }
        /// <summary>
        /// A serialized JSON string representing the description of the product.
        /// This allows for flexible storage of the product description, which may contain multiple parts.
        /// </summary>
        [Column(TypeName = "nvarchar(MAX)")]
        public string DescriptionSerialized { get; set; }
        /// <summary>
        /// Gets or sets the product description as an array of strings.
        /// The description is serialized and deserialized to and from the <see cref="DescriptionSerialized"/> property.
        /// </summary>
        [NotMapped]
        public string[] Description
        {
            get => string.IsNullOrEmpty(DescriptionSerialized)? new string[0] : JsonConvert.DeserializeObject<string[]>(DescriptionSerialized);
            set =>DescriptionSerialized=JsonConvert.SerializeObject(value);
        }
        /// <summary>
        /// A collection of reviews for this product.
        /// </summary>
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        /// <summary>
        /// The average rating of the product, calculated from the reviews.
        /// Returns 0 if there are no reviews.
        /// </summary>
        [NotMapped]
        public double AverageRating => Reviews.Any() ? Reviews.Average(r => r.Rating) : 0;
        /// <summary>
        /// Binary data for the product's image (e.g., JPEG, PNG).
        /// </summary>
        public byte[] ImageData { get; set; }
        /// <summary>
        /// MIME type of the product's image (e.g., "image/jpeg", "image/png").
        /// </summary>
        public string MimeType { get; set; }
        /// <summary>
        /// A collection of cart items that reference this product.
        /// </summary>
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
