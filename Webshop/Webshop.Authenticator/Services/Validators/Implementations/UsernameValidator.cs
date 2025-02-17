using Microsoft.EntityFrameworkCore;
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
    /// This is the implementation of UserValidation for IValidationManager. Checks the availablity of username in database and the correctivity of the username.
    /// </summary>
    public class UsernameValidator : IValidationManager
    {
        private IUserManager userManager;

        public UsernameValidator(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        public string IsAvailable(string key)
        {

            if(key is null)
            {
                return "Hiba! Nem adtál meg felhasználónevet!";
            }
            IQueryable<UserData> users = userManager.GetUsers();
            var usernames = users.Select(x => x.Username);
            if(usernames.Contains(key))
            {
                return "Hiba! A felhasználónév már regisztrálva van.";
            }
            if(key.Length<6)
            {
                return "Hiba! A felhasználónév nem lehet rövidebb 6 karakternél.";
            }
            return "200";
        }
    }
}
