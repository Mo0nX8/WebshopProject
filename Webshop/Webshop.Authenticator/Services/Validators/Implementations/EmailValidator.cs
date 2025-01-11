using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Services.Interfaces_For_Services;

namespace Webshop.Services.Services.Validators.Implementations
{
    public class EmailValidator : IValidationManager
    {
        public bool IsAvailable(string email)
        {
            if (email == null)
            {
                return false;
            }
            if(!email.Contains('@'))
            {
                return false;
            }
            return true;
        }
    }
}
