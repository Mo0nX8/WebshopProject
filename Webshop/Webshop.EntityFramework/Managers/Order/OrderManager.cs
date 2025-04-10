using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Order
{
    /// <summary>
    /// Implementation of the IOrderManager interface for managing orders.
    /// Provides methods to add, remove, and retrieve orders from the repository.
    /// </summary>
    public class OrderManager : IOrderManager
    {
        private IOrderRepository _orderRepository;

        /// <summary>
        /// Constructor to initialize the OrderManager with an IOrderRepository.
        /// </summary>
        /// <param name="orderRepository">The repository used to interact with orders in the database.</param>
        public OrderManager(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// Adds a new order by delegating the operation to the order repository.
        /// </summary>
        /// <param name="order">The order to be added.</param>
        public void AddOrder(Orders order)
        {
            if (order is not null)
            {
                _orderRepository.AddOrder(order);
            }
        }

        /// <summary>
        /// Removes an existing order by delegating the operation to the order repository.
        /// </summary>
        /// <param name="order">The order to be removed.</param>
        public void RemoveOrder(Orders order)
        {
            if (order is not null)
            {
                _orderRepository.RemoveOrder(order);
            }
        }

        /// <summary>
        /// Retrieves all orders from the repository.
        /// </summary>
        /// <returns>An IQueryable collection of orders.</returns>
        public IQueryable<Orders> GetOrders()
        {
            return _orderRepository.GetOrders();
        }
    }
}
