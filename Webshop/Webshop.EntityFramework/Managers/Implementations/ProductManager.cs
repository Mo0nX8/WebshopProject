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

        public void Add(Storage storage)
        {
            _context.StorageData.Add(storage);
        }

        public IQueryable<Storage> GetProduct()
        {
            return _context.StorageData.AsQueryable();
        }
    }
}
