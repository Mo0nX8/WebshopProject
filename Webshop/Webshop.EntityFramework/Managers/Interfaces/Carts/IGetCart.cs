using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;
using CartEntity = Webshop.EntityFramework.Data.ShoppingCart;

namespace Webshop.EntityFramework.Managers.Interfaces.Cart
{
    public interface IGetCart
    {
        List<Products> GetProduct(int cartId);
        CartEntity GetCart(int? cartId);
    }
}
