using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Order
{
    /// <summary>
    /// This interface manages the orders
    /// </summary>
    public interface IOrderRemover
    {
        /// <summary>
        /// This method requires an order as parameter. It remove the specific order from the database.
        /// </summary>
        /// <param name="order"></param>
        void RemoveOrder(Orders order);
    }
}
