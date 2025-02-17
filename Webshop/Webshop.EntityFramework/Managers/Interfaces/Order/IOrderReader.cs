using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Interfaces.Order
{
    /// <summary>
    /// This interface manages the orders
    /// </summary>
    public interface IOrderReader
    {
        /// <summary>
        /// This method returns all orders from the database.
        /// </summary>
        /// <returns></returns>
        IQueryable<Orders> GetOrders();
    }
}
