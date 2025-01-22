using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Managers.Interfaces.User;
using Webshop.Services.Interfaces_For_Services;

namespace Webshop.Services.Services.Validators.Implementations
{
    public class EmailValidator : IValidationManager
    {

        public string IsAvailable(string email)
        {
            if (email == null)
            {
                return "201";
            }
            if(!email.Contains('@'))
            {
                return "202";
            }
            return "200";
        }
    }
}
