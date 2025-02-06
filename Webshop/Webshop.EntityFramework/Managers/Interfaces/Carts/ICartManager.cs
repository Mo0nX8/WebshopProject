using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Interfaces.Cart
{
    public interface ICartManager : ICartRemover, IGetCart
    {
        void AddCart(ShoppingCart cart);
    }
}
