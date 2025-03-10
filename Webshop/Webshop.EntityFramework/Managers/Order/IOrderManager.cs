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
    public interface IOrderManager : IOrderReader
    {
        /// <summary>
        /// This method requires an order as parameter. It adds the order given to it to the database.
        /// </summary>
        /// <param name="order"></param>
        void AddOrder(Orders order);

    }

}
