using Microsoft.EntityFrameworkCore;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Order;
using Webshop.Services.Interfaces;

namespace Webshop.Services.Services.OrderService
{
     /// <summary>
     /// This is the implementation for <see cref="IOrderServices"/>.
     /// Provides methods for handling orders.
     /// </summary>>
    public class OrderServices : IOrderServices
    {
        private IOrderManager orderManager;
        /// <summary>
        ///  Constructor for initializing <see cref="OrderServices"/> with dependencies.
        /// </summary>
        /// <param name="orderManager">Service to manage orders.</param>
        public OrderServices(IOrderManager orderManager)
        {
            this.orderManager = orderManager;
        }
        /// <summary>
        /// Retrieve user's orders.
        /// </summary>
        /// <param name="userId">The user's id. </param>
        /// <returns><see cref="List{Orders}"/> of orders</returns>
        public List<Orders> GetOrders(int userId)
        {
            var orders = orderManager.GetOrders()
                 .Where(o => o.UserId == userId)
                 .Include(o=>o.OrderItems)
                 .ThenInclude(o=>o.Product)
                 .ToList();
            if (orders is null)
            {
                orders = new List<Orders>();
            }
            return orders;
        }
    }
}
