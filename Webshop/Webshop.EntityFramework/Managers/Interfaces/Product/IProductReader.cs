using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Interfaces.Product
{
    public interface IProductReader
    {
        IQueryable<Products> GetProduct();
    }
}
