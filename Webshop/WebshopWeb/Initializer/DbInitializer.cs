using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;
using Webshop.EntityFramework;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Implementations;
using Webshop.EntityFramework.Managers.Interfaces.User;
using Webshop.Services.Interfaces_For_Services;

namespace WebshopWeb.Initializer
{
    /// <summary>
    /// This class is for the Database Seeding process.
    /// </summary>
    public class DbInitializer
    {

        /// <summary>
        /// This method using Dependency Injection. It requires ApplicationBuilder, UserManager, EncryptManager, DbContext and a path to run. 
        /// It loads data from a json to the database. This fills up the product table.
        /// This method seeds an admin user into the database.
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <param name="userManager"></param>
        /// <param name="encryptManager"></param>
        /// <param name="_context"></param>
        /// <param name="path"></param>
        public static void Seed(IApplicationBuilder applicationBuilder, IUserManager userManager, IEncryptManager encryptManager, GlobalDbContext _context, string path, IConfiguration configuration)
        {
            using(var serviceScope=applicationBuilder.ApplicationServices.CreateScope())
            { 
                _context.Database.EnsureCreated();
                IQueryable<UserData> users = userManager.GetUsers();
                var user=users.FirstOrDefault(x=>x.IsAdmin==true);
                if (user is null)
                {
                    UserData newUser = new UserData();
                    newUser.EmailAddress = configuration["Adminuser:Email"];
                    newUser.Username = configuration["Adminuser:Password"];
                    newUser.IsAdmin = true;
                    newUser.Password = encryptManager.Hash("Admin123");
                    user=newUser; 
                    _context.Users.Add(newUser);
                    _context.SaveChanges();
                    ShoppingCart cart = new ShoppingCart();
                    cart.UserId = user.Id;
                    cart.CartItems = new List<CartItem>();
                    _context.Carts.Add(cart);
                    _context.SaveChanges();
                }


                var json = File.ReadAllText(path, Encoding.Default);
                var products=JsonConvert.DeserializeObject<List<Products>>(json);
                if(products.Count()!=_context.StorageData.Count())
                {
                    var productImagePath = "wwwroot/images";
                    foreach (var product in products)
                    {
                        var existingProduct = _context.StorageData.FirstOrDefault(p => p.ProductName == product.ProductName);
                        if (existingProduct is null)
                        {
                            product.DescriptionSerialized = JsonConvert.SerializeObject(product.Description);
                            if (product.ImageData == null)
                            {
                                string normalizedProductName = Regex.Replace(product.ProductName, "[()/\"]", "");
                                string pathOfImage = Path.Combine(Directory.GetCurrentDirectory(), productImagePath, $"{normalizedProductName}_1.jpg");
                                if (File.Exists(pathOfImage))
                                {

                                    product.ImageData = File.ReadAllBytes(pathOfImage);

                                }
                                product.MimeType = "image/jpeg";

                            }
                            _context.StorageData.Add(product);
                            ;
                        }
                    }
                }
                
                _context.SaveChanges();
                

            }
        }
    }
}
