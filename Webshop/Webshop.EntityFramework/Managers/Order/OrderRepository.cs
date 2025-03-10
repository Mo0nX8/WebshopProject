using Microsoft.EntityFrameworkCore;
using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Order
{
    public class OrderRepository : IOrderRepository
    {
        private GlobalDbContext _context;

        public OrderRepository(GlobalDbContext context)
        {
            _context = context;
        }

        public void AddOrder(Orders order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public IQueryable<Orders> GetOrders()
        {
            return _context.Orders.AsQueryable();
        }

        public void RemoveOrder(Orders order)
        {
            _context.Orders.Remove(order);
        }
    }
}
