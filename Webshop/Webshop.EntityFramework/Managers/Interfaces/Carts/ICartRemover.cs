using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.EntityFramework.Managers.Interfaces.Cart
{
    public interface ICartRemover
    {
        void RemoveItemFromCart(int? cartId, int productId);
    }
}
