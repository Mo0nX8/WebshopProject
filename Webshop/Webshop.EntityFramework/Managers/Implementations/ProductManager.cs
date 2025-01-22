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

        public IQueryable<Products> GetProduct()
        {
            return _context.StorageData.AsQueryable();
        }
    }
}
