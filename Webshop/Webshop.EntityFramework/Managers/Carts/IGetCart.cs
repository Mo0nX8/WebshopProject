using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;
using CartEntity = Webshop.EntityFramework.Data.ShoppingCart;

namespace Webshop.EntityFramework.Managers.Carts
{
    /// <summary>
    /// Get cart's datas
    /// </summary>
    public interface IGetCart
    {
        /// <summary>
        /// Returns List of Products from a cart by cartId
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        List<Products> GetProduct(int cartId);
        /// <summary>
        /// Return a specific cart by cartId
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        CartEntity GetCart(int? cartId);
    }
}
