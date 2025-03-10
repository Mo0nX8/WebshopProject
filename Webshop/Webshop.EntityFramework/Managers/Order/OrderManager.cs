using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Order
{
    /// <summary>
    /// This is the implementation for IOrderManager.
    /// </summary>
    public class OrderManager : IOrderManager
    {
        private IOrderRepository _orderRepository;

        public OrderManager(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public void AddOrder(Orders order)
        {
            if (order is not null)
            {
                _orderRepository.AddOrder(order);
            }

        }
        public void RemoveOrder(Orders order)
        {
            if (order is not null)
            {
                _orderRepository.RemoveOrder(order);
            }

        }

        public IQueryable<Orders> GetOrders()
        {
            return _orderRepository.GetOrders();
        }
    }
}
