using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Services.Services.Register;

namespace Webshop.Services.Interfaces
{
    public interface IRegisterService
    {
        ValidationResultModel ValidateEmail(string email);
        ValidationResultModel ValidatePassword(string password1, string password2);
        ValidationResultModel ValidateUsername(string username);
        bool RegisterUser(string email, string password, string username);
    }
}
