using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Reviews
{
    /// <summary>
    /// Implementation of the IReviewManager interface. 
    /// This class manages the logic for handling reviews.
    /// </summary>
    public class ReviewManager : IReviewManager
    {
        private readonly IReviewRepository _reviewRepository;

        /// <summary>
        /// Constructor to initialize the ReviewManager with the review repository.
        /// </summary>
        /// <param name="reviewRepository">The repository used to interact with review data.</param>
        public ReviewManager(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        /// <summary>
        /// Adds a new review by delegating the operation to the review repository.
        /// </summary>
        /// <param name="review">The review to be added.</param>
        public void AddReview(Review review)
        {
            _reviewRepository.AddReview(review);
        }
    }
}
