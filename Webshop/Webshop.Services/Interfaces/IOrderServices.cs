using Webshop.EntityFramework.Data;

namespace Webshop.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for order-related services.
    /// </summary>
    public interface IOrderServices
    {
        /// <summary>
        /// Retrieves a list of orders for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose orders are to be retrieved.</param>
        /// <returns>A list of orders associated with the given user.</returns>
        List<Orders> GetOrders(int userId);
    }
}
