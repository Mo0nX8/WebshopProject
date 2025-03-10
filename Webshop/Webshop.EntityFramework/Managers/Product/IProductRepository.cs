using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Product
{
    public interface IProductRepository
    {
        void AddProduct(Products storage);
        int CountProducts();
        Products GetProduct(int id);
        IQueryable<Products> GetProducts();
    }
}
