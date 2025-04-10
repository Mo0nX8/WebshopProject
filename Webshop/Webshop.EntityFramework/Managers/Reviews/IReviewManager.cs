using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Reviews
{
    /// <summary>
    /// Interface for the ReviewManager, defining the methods for managing reviews.
    /// </summary>
    public interface IReviewManager
    {
        /// <summary>
        /// Adds a new review.
        /// </summary>
        /// <param name="review">The review to be added.</param>
        void AddReview(Review review);
    }
}
