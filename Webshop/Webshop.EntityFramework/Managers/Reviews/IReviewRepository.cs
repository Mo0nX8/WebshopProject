using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Reviews
{
    /// <summary>
    /// Interface for the ReviewRepository, defining methods for interacting with review data.
    /// </summary>
    public interface IReviewRepository
    {
        /// <summary>
        /// Adds a new review to the repository.
        /// </summary>
        /// <param name="review">The review to be added.</param>
        void AddReview(Review review);
    }
}
