using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Carts;
using Webshop.EntityFramework.Managers.User;
using Webshop.Services.Interfaces;

namespace Webshop.Services.Services.Register
{
    public class RegisterService 
    {
        private readonly IValidationManager emailValidator;
        private readonly IValidationManager usernameValidator;
        private readonly IValidationManager passwordValidator;
        private readonly IUserManager userManager;
        private readonly ICartManager cartManager;
        private readonly IEncryptManager encryptManager;

        public RegisterService(IValidationManager emailValidator, IValidationManager usernameValidator, IValidationManager passwordValidator, IUserManager userManager, ICartManager cartManager, IEncryptManager encryptManager)
        {
            this.emailValidator = emailValidator;
            this.usernameValidator = usernameValidator;
            this.passwordValidator = passwordValidator;
            this.userManager = userManager;
            this.cartManager = cartManager;
            this.encryptManager = encryptManager;
        }
        public string ValidateEmail(string email)
        {
            var result = "";
            string emailValidatorResult = emailValidator.IsAvailable(email);
            if (emailValidatorResult != "200")
            {
                result = emailValidatorResult;
            }
            return result;
        }
        public string ValidateUsername(string username)
        {
            var result = "";
            string usernameValidatorResult = usernameValidator.IsAvailable(username);
            if (usernameValidatorResult != "200")
            {
                result = usernameValidatorResult;
            }
            return result;
        }
        public string ValidatePassword(string password1, string password2)
        {
            var result = "";
            if (password1 != password2)
            {
                result = "A két jelszó nem egyezik!";
            }
            string passwordValidatorResult = passwordValidator.IsAvailable(password1);
            if (passwordValidatorResult != "200")
            {
                result = passwordValidatorResult;
            }
            return result;
        }
        //public bool TryRegister(string username, string email, string password1, string password2)
        //{

        //    string emailResponseCode = ValidateEmail(email);
        //    string usernameResponseCode = usernameValidator.IsAvailable(username);
        //    string passwordResponseCode = "";
        //    if (password1 == password2)
        //    {
        //        passwordResponseCode = passwordValidator.IsAvailable(password1);
        //    }
        //    else
        //    {
        //        passwordResponseCode = "A két jelszó nem egyezik!";
        //        ModelState.AddModelError("Password", passwordResponseCode);
        //    }
        //    if (emailResponseCode != "200")
        //    {
        //        ModelState.AddModelError("EmailAddress", emailResponseCode);
        //    }
        //    if (usernameResponseCode != "200")
        //    {
        //        ModelState.AddModelError("Username", usernameResponseCode);
        //    }
        //    if (ModelState.ErrorCount > 1)
        //    {
        //        return View("Register");
        //    }

        //    UserData user = new UserData()
        //    {
        //        Username = username,
        //        EmailAddress = email,
        //        Password = encryptManager.Hash(password1)
        //    };

        //    userManager.AddUser(user);
        //    ShoppingCart cart = new ShoppingCart()
        //    {
        //        UserId = user.Id,
        //        CartItems = new List<CartItem>()
        //    };
        //    cartManager.AddCart(cart);

        //    return RedirectToAction("Login");

        //}
    }
}
