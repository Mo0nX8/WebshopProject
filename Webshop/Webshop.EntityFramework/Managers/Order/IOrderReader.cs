using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Order
{
    /// <summary>
    /// This interface manages the orders
    /// </summary>
    public interface IOrderReader
    {
        /// <summary>
        /// This method returns all orders from the database.
        /// </summary>
        /// <returns>IQueryable collection of orders</returns>
        IQueryable<Orders> GetOrders();
    }
}
