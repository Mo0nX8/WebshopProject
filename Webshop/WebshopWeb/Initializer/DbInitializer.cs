using Webshop.EntityFramework;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Implementations;
using Webshop.EntityFramework.Managers.Interfaces.User;
using Webshop.Services.Interfaces_For_Services;

namespace WebshopWeb.Initializer
{
    public class DbInitializer
    {

        public static void Seed(IApplicationBuilder applicationBuilder, IUserManager userManager, IEncryptManager encryptManager, GlobalDbContext _context)
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
                    cart.Products = new List<Products>();
                    _context.Carts.Add(cart);
                    _context.SaveChanges();
                }

               

            }
        }
    }
}
