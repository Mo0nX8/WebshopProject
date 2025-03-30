using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Reviews;

namespace Webshop.UnitTests
{
    [TestFixture]
    public class ReviewServiceTests
    {
        private Mock<IReviewRepository> _reviewRepositoryMock;
        private IReviewManager _reviewManager;

        [SetUp]
        public void Setup()
        {
            _reviewRepositoryMock = new Mock<IReviewRepository>();
            _reviewManager=new ReviewManager( _reviewRepositoryMock.Object );
        }

        [Test]
        public void TestThatAddReviewShouldCallAddReviewOnce()
        {
            var review = new Review
            {
                Id = 1,
                ProductId = 12,
                Comment = "Test",
                Rating = 4,
                CreatedAt = DateTime.Now,
            };
            _reviewManager.AddReview(review);

            _reviewRepositoryMock.Verify(r => r.AddReview(review),Times.Once);
        }
    }
}
