using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Order
{
    /// <summary>
    /// This interface manages the orders
    /// </summary>
    public interface IOrderManager : IOrderReader
    {
        /// <summary>
        /// This method requires an order as parameter. It adds the order given to it to the database.
        /// </summary>
        /// <param name="order">The object which should be added to the database.</param>
        void AddOrder(Orders order);

    }

}
