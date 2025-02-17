using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Interfaces.Cart
{
    /// <summary>
    /// This is a CartManager
    /// </summary>
    public interface ICartManager : ICartRemover, IGetCart
    {
        /// <summary>
        /// This method adds a cart to the database. It requires a Cart parameter.
        /// </summary>
        /// <param name="cart"></param>
        void AddCart(ShoppingCart cart);
    }
}
