using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Carts
{
    public interface ICartRepository
    {
        void AddCart(ShoppingCart cart);
        ShoppingCart? GetCart(int cartId);
        void AddProductsToCart(int userId, List<int> productIds);
        List<Products> GetProducts(int cartId);
        void RemoveItemFromCart(int cartId, int productId);
    }
}
