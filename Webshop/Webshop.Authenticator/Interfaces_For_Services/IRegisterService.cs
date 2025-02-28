using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Services.Interfaces_For_Services
{
    public interface IRegisterService
    {
        ValidationResult ValidateEmail(string email);
        ValidationResult ValidatePassword(string password1, string password2);
        ValidationResult ValidateUsername(string username);
        bool RegisterUser(string email, string password, string username);
    }
}
