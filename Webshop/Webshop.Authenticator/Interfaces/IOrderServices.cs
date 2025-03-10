using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;

namespace Webshop.Services.Interfaces
{
    public interface IOrderServices
    {
        List<Orders> GetOrders(int userId);
    }
}
