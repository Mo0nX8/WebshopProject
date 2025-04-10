using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Order
{
    /// <summary>
    /// Repository for managing orders in the database.
    /// Provides methods for adding, removing, and retrieving orders.
    /// </summary>
    public class OrderRepository : IOrderRepository
    {
        private GlobalDbContext _context;

        /// <summary>
        /// Initializes a new instance of the OrderRepository class with the provided DbContext.
        /// </summary>
        /// <param name="context">The DbContext used for database operations.</param>
        public OrderRepository(GlobalDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new order to the database.
        /// </summary>
        /// <param name="order">The order to be added to the database.</param>
        public void AddOrder(Orders order)
        {
            // Add the order to the Orders DbSet and save changes to the database.
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        /// <summary>
        /// Retrieves all orders from the database.
        /// </summary>
        /// <returns>An IQueryable collection of orders from the database.</returns>
        public IQueryable<Orders> GetOrders()
        {
            return _context.Orders.AsQueryable();
        }

        /// <summary>
        /// Removes an existing order from the database.
        /// </summary>
        /// <param name="order">The order to be removed from the database.</param>
        public void RemoveOrder(Orders order)
        {
            _context.Orders.Remove(order);
        }
    }
}
