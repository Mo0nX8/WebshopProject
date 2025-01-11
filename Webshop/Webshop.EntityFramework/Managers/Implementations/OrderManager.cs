using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Interfaces.Order;

namespace Webshop.EntityFramework.Managers.Implementations
{
    public class OrderManager : IOrderManager
    {
        private GlobalDbContext _dbContext;

        public OrderManager(GlobalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Orders order)
        {
            _dbContext.Orders.Add(order);
        }
    }
}
