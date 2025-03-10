using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Reviews
{
    public class ReviewManager : IReviewManager
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewManager(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public void AddReview(Review review)
        {
            _reviewRepository.AddReview(review);
        }
    }
}
