using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly UsernameValidator usernameValidator;
        private readonly PasswordValidator passwordValidator;
        public AuthenticationController(IUserManager userManager, IEncryptManager encryptManager, IAuthenticationManager authenticationManager, IServiceProvider serviceProvider)
        {
            this.userManager = userManager;
            this.encryptManager = encryptManager;
            this.authenticationManager = authenticationManager;
            this.emailValidator = serviceProvider.GetService<EmailValidator>();
            this.usernameValidator = serviceProvider.GetService<UsernameValidator>();
            this.passwordValidator = serviceProvider.GetService<PasswordValidator>();
        }

        public IActionResult Login()
        {
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
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Register");
        }
        public IActionResult TryRegister(UserData user)
        {
            string emailResponseCode = emailValidator.IsAvailable(user.EmailAddress);
            string usernameResponseCode = usernameValidator.IsAvailable(user.Username);
            string passwordResponseCode = passwordValidator.IsAvailable(user.Password);
            if(emailResponseCode=="200")
            {
                if(usernameResponseCode=="200")
                {
                    if(passwordResponseCode=="200")
                    {
                        user.Password = encryptManager.Hash(user.Password);
                        userManager.Add(user);
                        return RedirectToAction("Login");
                    }
                    TempData["Code"] = passwordResponseCode;
                   
                }
                else
                {
                    TempData["Code"] = usernameResponseCode;
                }

                 
            }
            else
            {
                TempData["Code"] = emailResponseCode;
            }
            
            return RedirectToAction("Register");
           
        }
        public IActionResult Logout()
        {
            authenticationManager.LogOut();
            return RedirectToAction("Index","Home");
        }
    }
}
