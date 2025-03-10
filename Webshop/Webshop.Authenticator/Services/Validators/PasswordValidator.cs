using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Services.Interfaces;

namespace Webshop.Services.Services.Validators
{
    /// <summary>
    /// This is the implementation of PasswordValidation for IValidationManager. Checks the availablity of password through some security checks.
    /// </summary>
    public class PasswordValidator : IValidationManager
    {
        public string IsAvailable(string key)
        {
            if (key == null)
            {
                return "Hiba! Nem adtál meg jelszót!";
            }
            if (!key.Any(char.IsDigit))
            {
                return "Hiba! A jelszónak tartalmaznia kell legaláb egy számot!";
            }
            if (key.Length < 8)
            {
                return "Hiba! A jelszónak legalább 8 karakter hosszúnak kell lennie!";
            }
            return "200";
        }
    }
}
