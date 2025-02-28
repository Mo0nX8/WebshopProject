using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Interfaces.Cart;
using Webshop.EntityFramework.Managers.Interfaces.User;
using Webshop.Services.Interfaces_For_Services;

namespace Webshop.Services.Services.Register
{
    public class RegisterService : IRegisterService
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
        public ValidationResult ValidateEmail(string email) 
        {
            var result=new ValidationResult();
            string emailValidatorResult = emailValidator.IsAvailable(email);
            if (emailValidatorResult != "200") 
            {
                result.AddError(emailValidatorResult);
            }
            return result;
        }
        public ValidationResult ValidateUsername(string username)
        {
            var result = new ValidationResult();
            string usernameValidatorResult = usernameValidator.IsAvailable(username);
            if (usernameValidatorResult != "200")
            {
                result.AddError(usernameValidatorResult);
            }
            return result;
        }
        public ValidationResult ValidatePassword(string password1, string password2)
        {
            var result = new ValidationResult();
            if(password1!=password2)
            {
                result.AddError("A két jelszó nem egyezik!");
            }
            string passwordValidatorResult = passwordValidator.IsAvailable(password1);
            if (passwordValidatorResult != "200")
            {
                result.AddError(passwordValidatorResult);
            }
            return result;
        }
        public bool RegisterUser(string email, string password, string username)
        {
            UserData user = new UserData()
            {
                Username = username,
                Password = encryptManager.Hash(password),
                EmailAddress = email
            };
            userManager.AddUser(user);
            ShoppingCart cart = new ShoppingCart()
            {
                UserId = user.Id,
                CartItems = new List<CartItem>()
            };
            cartManager.AddCart(cart);
            return true;
        }

        System.ComponentModel.DataAnnotations.ValidationResult IRegisterService.ValidateEmail(string email)
        {
            throw new NotImplementedException();
        }

        System.ComponentModel.DataAnnotations.ValidationResult IRegisterService.ValidatePassword(string password1, string password2)
        {
            throw new NotImplementedException();
        }

        System.ComponentModel.DataAnnotations.ValidationResult IRegisterService.ValidateUsername(string username)
        {
            throw new NotImplementedException();
        }
    }
}
