using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;
using CartEntity = Webshop.EntityFramework.Data.Cart;

namespace Webshop.EntityFramework.Managers.Interfaces.Cart
{
    public interface IGetCart
    {
        CartEntity GetProduct(int cartId);
    }
}
