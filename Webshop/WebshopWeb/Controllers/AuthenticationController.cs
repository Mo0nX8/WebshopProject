using Microsoft.AspNetCore.Mvc;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Interfaces.User;
using Webshop.Services.Interfaces_For_Services;

namespace WebshopWeb.Controllers
{
    public class AuthenticationController : Controller
    {
        private IUserManager userManager;
        private IEncryptManager encryptManager;
        private IAuthenticationManager authenticationManager;
        public AuthenticationController(IUserManager userManager, IEncryptManager encryptManager, IAuthenticationManager authenticationManager)
        {
            this.userManager = userManager;
            this.encryptManager = encryptManager;
            this.authenticationManager = authenticationManager;
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult TryLog(string email, string password)
        {
            if (authenticationManager.TryLogin(email, password))
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Register");
        }
        [HttpPost]
        public IActionResult TryRegister(UserData user)
        {
            user.Password = encryptManager.Hash(user.Password);
            userManager.Add(user);
            
            return RedirectToAction("Login");
        }
        public IActionResult Logout()
        {
            authenticationManager.LogOut();
            return RedirectToAction("Index","Home");
        }
    }
}
