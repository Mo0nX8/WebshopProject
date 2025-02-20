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
        private GlobalDbContext _context;

        public ProductManager(GlobalDbContext context)
        {
            _context = context;
        }

        public void Add(Products storage)
        {
            _context.StorageData.Add(storage);
            _context.SaveChanges();
        }

        public Products GetProduct(int id)
        {
            return _context.StorageData.FirstOrDefault(p=>p.Id==id);
        }

        public IQueryable<Products> GetProducts()
        {
            return _context.StorageData.AsQueryable();
        }
    }
}
