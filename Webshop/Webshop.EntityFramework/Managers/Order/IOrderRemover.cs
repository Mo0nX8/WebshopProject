using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Order
{
    /// <summary>
    /// This interface manages the orders
    /// </summary>
    public interface IOrderRemover
    {
        /// <summary>
        /// This method requires an order as parameter. It removes the specific order from the database.
        /// </summary>
        /// <param name="order">Order object which should be removed.</param>
        void RemoveOrder(Orders order);
    }
}
