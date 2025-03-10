using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Reviews
{
    public interface IReviewRepository
    {
        void AddReview(Review review);
    }
}
