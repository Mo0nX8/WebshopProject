using Microsoft.AspNetCore.Mvc;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Interfaces.User;
using Webshop.Services.Interfaces_For_Services;
using Webshop.Services.Services.Validators.Implementations;

namespace WebshopWeb.Controllers
{
    public class AuthenticationController : Controller
    {
        private IUserManager userManager;
        private IEncryptManager encryptManager;
        private IAuthenticationManager authenticationManager;
        private readonly EmailValidator emailValidator;
        public AuthenticationController(IUserManager userManager, IEncryptManager encryptManager, IAuthenticationManager authenticationManager,IServiceProvider serviceProvider)
        {
            this.userManager = userManager;
            this.encryptManager = encryptManager;
            this.authenticationManager = authenticationManager;
            this.emailValidator = serviceProvider.GetService<EmailValidator>();
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            ViewBag.EmailResponse = TempData["Code"];
            return View();
        }
        [HttpPost]
        public IActionResult TryLog(string email, string password)
        {
            if (authenticationManager.TryLogin(email, password))
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Register");
        }
        public IActionResult TryRegister(UserData user)
        {
            if(1>2)
            {
                user.Password = encryptManager.Hash(user.Password);
                userManager.Add(user);
                return RedirectToAction("Login");
            }
            TempData["Code"] = emailValidator.IsAvailable(user.EmailAddress);
            return RedirectToAction("Register");
           
        }
        public IActionResult Logout()
        {
            authenticationManager.LogOut();
            return RedirectToAction("Index","Home");
        }
    }
}
