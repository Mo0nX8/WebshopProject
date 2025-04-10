using Microsoft.EntityFrameworkCore;
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
    /// Provides method for handling username validation.
    /// </summary>>
    public class UsernameValidator : IValidationManager
    {
        private IUserManager userManager;
        /// <summary>
        /// Constructor for initializing <see cref="ProductService"/> with dependencies.
        /// </summary>
        /// <param name="userManager">Service for user management.</param>
        public UsernameValidator(IUserManager userManager)
        {
            this.userManager = userManager;
        }
        /// <summary>
        /// Checks if the provided username is available for registration.
        /// </summary>
        /// <param name="key">The username to validate.</param>
        /// <returns>
        /// Returns an error message if the username is invalid or already in use, otherwise returns "200" indicating success.
        /// </returns>
        public string IsAvailable(string key)
        {

            if (key is null)
            {
                return "Hiba! Nem adtál meg felhasználónevet!";
            }
            IQueryable<UserData> users = userManager.GetUsers();
            if (users.Select(x => x.Username).Contains(key))
            {
                return "Hiba! A felhasználónév már regisztrálva van.";
            }
            if (key.Length < 6)
            {
                return "Hiba! A felhasználónév nem lehet rövidebb 6 karakternél.";
            }
            return "200";
        }
    }
}
