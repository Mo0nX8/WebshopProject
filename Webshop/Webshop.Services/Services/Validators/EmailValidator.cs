using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.User;
using Webshop.Services.Interfaces;

namespace Webshop.Services.Services.Validators
{
    /// <summary>
    /// This is the implementation for <see cref="IValidationManager"/>.
    /// Provides method for handling email validation.
    /// </summary>>
    public class EmailValidator : IValidationManager
    {
        private IUserManager userManager;
        /// <summary>
        /// Constructor for initializing <see cref="ProductService"/> with dependencies.
        /// </summary>
        /// <param name="userManager">Service for user management.</param>
        public EmailValidator(IUserManager userManager)
        {
            this.userManager = userManager;
        }
        /// <summary>
        /// Checks if the provided email is available for registration.
        /// </summary>
        /// <param name="email">The email address to validate.</param>
        /// <returns>
        /// Returns an error message if the email is invalid or already in use, otherwise returns "200" indicating success.
        /// </returns>
        public string IsAvailable(string email)
        {
            if (email == null)
            {
                return "Hiba! Nem adtál meg email címet!";
            }
            if (!email.Contains('@'))
            {

                return "Hiba! Nem érvényes email cím!";

            }
            IQueryable<UserData> users = userManager.GetUsers();

            if (email.EndsWith(".com") || email.EndsWith(".hu"))
            {
                if (users.Select(x => x.EmailAddress).Contains(email))
                {
                    return "Hiba! Az email cím használatban van!";
                }
            }
            else
            {
                return "Hiba! Nem érvényes email cím!";
            }
            return "200";
        }
    }
}
