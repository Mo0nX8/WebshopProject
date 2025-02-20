using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Interfaces.User;
using Webshop.Services.Interfaces_For_Services;

namespace Webshop.Services.Services.Validators.Implementations
{
    /// <summary>
    /// This is the implementation of EmailValidation for IValidationManager. Checks the availablity of email in database.
    /// </summary>
    public class EmailValidator : IValidationManager
    {
        private IUserManager userManager;

        public EmailValidator(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        public string IsAvailable(string email)
        {
            if (email == null)
            {
                return "Hiba! Nem adtál meg email címet!";
            }
            if(!email.Contains('@'))
            {
                
                return "Hiba! Nem érvényes email cím!";

            }
            if (!email.Contains(".com") || !email.Contains(".hu"))
            {
                return "Hiba! Nem érvényes email cím!";
            }
            IQueryable<UserData> users=userManager.GetUsers();
            if(users.Select(x => x.EmailAddress).Contains(email))
            {
                return "Hiba! Az email cím használatban van!";
            }
            return "200";
        }
    }
}
    