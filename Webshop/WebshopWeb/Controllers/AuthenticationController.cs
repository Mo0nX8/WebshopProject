using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webshop.EntityFramework;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Carts;
using Webshop.EntityFramework.Managers.User;
using Webshop.Services.Interfaces;
using Webshop.Services.Services.Validators;

namespace WebshopWeb.Controllers
{
    public class AuthenticationController : Controller
    {
        private GlobalDbContext _context;
        private ICartManager cartManager;
        private IUserManager userManager;
        private IEncryptManager encryptManager;
        private IAuthenticationManager authenticationManager;
        private readonly EmailValidator emailValidator;
        private readonly UsernameValidator usernameValidator;
        private readonly PasswordValidator passwordValidator;
        public AuthenticationController(IUserManager userManager, IEncryptManager encryptManager, IAuthenticationManager authenticationManager, IServiceProvider serviceProvider, ICartManager cartManager, GlobalDbContext context)
        {
            this.userManager = userManager;
            this.encryptManager = encryptManager;
            this.authenticationManager = authenticationManager;
            this.emailValidator = serviceProvider.GetService<EmailValidator>();
            this.usernameValidator = serviceProvider.GetService<UsernameValidator>();
            this.passwordValidator = serviceProvider.GetService<PasswordValidator>();
            this.cartManager = cartManager;
            _context = context;
        }

        public IActionResult Login()
        {
            ViewBag.Response = TempData["Code"];
            return View();
        }
        public IActionResult Register()
        {
            ViewBag.Response = TempData["Code"];
            return View();
        }
        [HttpPost]
        public IActionResult TryLog(string email, string password)
        {
            if (authenticationManager.TryLogin(email, password))
            {
                var user = userManager.GetUsers()
                    .Where(x => x.EmailAddress == email)
                    .Include(u=>u.Cart)
                    .FirstOrDefault();
                HttpContext.Session.SetInt32("UserId", user.Id);
                var cart = cartManager.GetCart(user.Cart.Id);
                HttpContext.Session.SetInt32("CartId",cart.Id);
                HttpContext.Session.SetString("IsAuthenticated", "True");
                Console.WriteLine(cart.Id);
                return RedirectToAction("Index", "Home");
            }
            TempData["Code"] = "Az email cím és jelszó páros nem egyezik!";
            return RedirectToAction("Login", "Authentication");
        }
        public IActionResult TryRegister(string username, string email, string password1, string password2)
        {

            string emailResponseCode = emailValidator.IsAvailable(email);
            string usernameResponseCode = usernameValidator.IsAvailable(username);
            string passwordResponseCode = "";
            if (password1==password2)
            {
                passwordResponseCode = passwordValidator.IsAvailable(password1);
            }
            else
            {
                passwordResponseCode = "A két jelszó nem egyezik!";
                ModelState.AddModelError("Password",passwordResponseCode);
            }
            if(emailResponseCode!="200")
            {
                ModelState.AddModelError("EmailAddress", emailResponseCode);
            }
            if (usernameResponseCode != "200")
            {
                ModelState.AddModelError("Username", usernameResponseCode);
            }
            if(ModelState.ErrorCount >1)
            {
                return View("Register");
            }

            UserData user = new UserData()
            {
                Username = username,
                EmailAddress = email,
                Password = encryptManager.Hash(password1)
            };

            userManager.AddUser(user);
            ShoppingCart cart = new ShoppingCart()
            {
                UserId = user.Id,
                CartItems = new List<CartItem>()
            };
            cartManager.AddCart(cart);

            return RedirectToAction("Login");

        }
        public IActionResult Logout()
        {
            authenticationManager.LogOut();
            HttpContext.Session.SetString("IsAuthenticated", "False");
            return RedirectToAction("Index","Home");
        }
    }
}
