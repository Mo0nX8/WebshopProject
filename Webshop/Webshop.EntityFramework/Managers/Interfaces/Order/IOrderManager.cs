using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Interfaces.Order
{
    public interface IOrderManager
    {
        void Add(Orders order);
        IQueryable<Orders> GetOrders();
    }

}
