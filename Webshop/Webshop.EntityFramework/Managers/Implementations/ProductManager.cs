using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Interfaces.Product;

namespace Webshop.EntityFramework.Managers.Implementations
{
    /// <summary>
    /// This is the implementation for IProductManager.
    /// </summary>
    public class ProductManager : IProductManager
    {
        private IProductRepository _productRepository;

        public ProductManager(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void AddProduct(Products storage)
        {
            if(storage is not null)
            {
                _productRepository.AddProduct(storage);
            }
        }

        public int CountProducts()
        {
            return _productRepository.CountProducts();
        }

        public Products GetProduct(int id)
        {
            if(id!=null)
            {
                return _productRepository.GetProduct(id);
            }
            return null;
        }

        public IQueryable<Products> GetProducts()
        {
            return _productRepository.GetProducts();
        }

    }
}
