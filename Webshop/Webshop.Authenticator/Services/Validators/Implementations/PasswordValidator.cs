using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Services.Interfaces_For_Services;

namespace Webshop.Services.Services.Validators.Implementations
{
    /// <summary>
    /// This is the implementation of PasswordValidation for IValidationManager. Checks the availablity of password through some security checks.
    /// </summary>
    public class PasswordValidator : IValidationManager
    {
        public string IsAvailable(string key)
        {
            if(key==null)
            {
                return "Hiba! Nem adtál meg jelszót!";
            }
            if (!key.Any(char.IsDigit) || key.Length<6)
            {
                return "Hiba! Rossz a megadott jelszó formátuma!";
            }
            return "200";
        }
    }
}
