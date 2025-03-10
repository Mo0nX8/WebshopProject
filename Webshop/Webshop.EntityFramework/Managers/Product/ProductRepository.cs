using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Product
{
    public class ProductRepository : IProductRepository
    {
        private GlobalDbContext _context;

        public ProductRepository(GlobalDbContext context)
        {
            _context = context;
        }

        public void AddProduct(Products storage)
        {
            _context.StorageData.Add(storage);
            _context.SaveChanges();
        }

        public int CountProducts()
        {
            return _context.StorageData.Count();
        }

        public Products GetProduct(int id)
        {
            return _context.StorageData.First(x => x.Id == id);
        }

        public IQueryable<Products> GetProducts()
        {
            return _context.StorageData.AsQueryable();
        }
    }
}
