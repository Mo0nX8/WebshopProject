using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Product
{
    /// <summary>
    /// This interface helps managing products.
    /// </summary>
    public interface IProductReader
    {
        /// <summary>
        /// This method requires an id as parameter. Returns a product by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Products GetProduct(int id);
        /// <summary>
        /// Retrieves all products from the database.
        /// </summary>
        /// <returns>IQueryable products collection.</returns>
        IQueryable<Products> GetProducts();
    }
}
