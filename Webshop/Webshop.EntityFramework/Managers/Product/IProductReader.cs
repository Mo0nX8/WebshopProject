using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Product
{
    /// <summary>
    /// This interface helps managing products.
    /// </summary>
    public interface IProductReader
    {
        /// <summary>
        /// This method requires an id as parameter. Returns a product by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Products GetProduct(int id);
        IQueryable<Products> GetProducts();
    }
}
