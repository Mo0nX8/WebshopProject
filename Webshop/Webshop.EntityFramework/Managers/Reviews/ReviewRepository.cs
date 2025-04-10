using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Reviews
{
    /// <summary>
    /// Implementation of the IReviewRepository interface.
    /// This class handles the interaction with the database to manage reviews.
    /// </summary>
    public class ReviewRepository : IReviewRepository
    {
        private readonly GlobalDbContext _context;

        /// <summary>
        /// Constructor to initialize the ReviewRepository with the database context.
        /// </summary>
        /// <param name="context">The database context used for interacting with the database.</param>
        public ReviewRepository(GlobalDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new review to the database.
        /// </summary>
        /// <param name="review">The review to be added.</param>
        public void AddReview(Review review)
        {
            _context.Reviews.Add(review);
            _context.SaveChanges();
        }
    }
}
