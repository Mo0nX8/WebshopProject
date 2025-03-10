using Microsoft.EntityFrameworkCore;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Order;
using Webshop.Services.Interfaces;

namespace Webshop.Services.Services.OrderService
{
    public class OrderServices : IOrderServices
    {
        private IOrderManager orderManager;

        public OrderServices(IOrderManager orderManager)
        {
            this.orderManager = orderManager;
        }
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
