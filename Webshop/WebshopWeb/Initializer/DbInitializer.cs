using Newtonsoft.Json;
using System.Text;
using Webshop.EntityFramework;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Implementations;
using Webshop.EntityFramework.Managers.Interfaces.User;
using Webshop.Services.Interfaces_For_Services;

namespace WebshopWeb.Initializer
{
    public class DbInitializer
    {

        public static void Seed(IApplicationBuilder applicationBuilder, IUserManager userManager, IEncryptManager encryptManager, GlobalDbContext _context, string path)
        {
            using(var serviceScope=applicationBuilder.ApplicationServices.CreateScope())
            { 
                _context.Database.EnsureCreated();
                IQueryable<UserData> users = userManager.GetUsers();
                var user=users.FirstOrDefault(x=>x.IsAdmin==true);
                if (user is null)
                {
                    UserData newUser = new UserData();
                    newUser.EmailAddress = "admin@dxmarket.hu";
                    newUser.Username = "admin";
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
                foreach (var product in products)
                {
                    var existingProduct = _context.StorageData.FirstOrDefault(p => p.ProductName == product.ProductName);
                    if (existingProduct is null)
                    {
                        product.DescriptionSerialized=JsonConvert.SerializeObject(product.Description);
                        _context.StorageData.Add(product);
                    }
                }
                _context.SaveChanges();

            }
        }
    }
}
