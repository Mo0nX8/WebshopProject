using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Order
{
    public interface IOrderRepository
    {
        void RemoveOrder(Orders order);
        void AddOrder(Orders order);
        IQueryable<Orders> GetOrders();
    }
}
