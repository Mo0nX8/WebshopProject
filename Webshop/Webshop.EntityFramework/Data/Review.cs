using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webshop.EntityFramework.Data
{
    /// <summary>
    /// This class represents reviews for a product.
    /// </summary>
    public class Review
    {
        /// <summary>
        /// Unique identifier for the review.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Foreign key referencing the product associated with the review.
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// This is the rating of the product.
        /// This property is set to be between 1-5.
        /// </summary>

        [Range(1, 5)]
        public int Rating { get; set; }
        /// <summary>
        /// Optional comment provided by the user.
        /// This field can be null, if the user choose to not leave a comment.
        /// </summary>

        public string? Comment { get; set; }
        /// <summary>
        /// The date and time when the review was created. 
        /// This is set to the current system time by default. 
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        /// <summary>
        /// Navigation property for the associated product.
        /// This allows access to the product details for the product being reviewed.
        /// </summary>

        [ForeignKey("ProductId")]
        public virtual Products Product { get; set; }
    }
}
