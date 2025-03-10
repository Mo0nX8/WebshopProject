using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Product
{
    /// <summary>
    /// This interface manages products
    /// </summary>
    public interface IProductManager : IProductRepository
    {
        /// <summary>
        /// This method requires a product as parameter. The method adds the product to the database.
        /// </summary>
        /// <param name="storage"></param>
        void AddProduct(Products storage);
        int CountProducts();
    }
}
