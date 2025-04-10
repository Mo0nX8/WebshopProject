using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Order
{
    /// <summary>
    /// Interface for interacting with the orders repository.
    /// Defines the contract for managing orders in the system.
    /// </summary>
    public interface IOrderRepository
    {
        /// <summary>
        /// Removes an existing order from the repository.
        /// </summary>
        /// <param name="order">The order to remove.</param>
        void RemoveOrder(Orders order);

        /// <summary>
        /// Adds a new order to the repository.
        /// </summary>
        /// <param name="order">The order to add.</param>
        void AddOrder(Orders order);

        /// <summary>
        /// Retrieves all orders in the system.
        /// </summary>
        /// <returns>An IQueryable collection of all orders.</returns>
        IQueryable<Orders> GetOrders();
    }
}
