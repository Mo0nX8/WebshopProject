using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Services.Interfaces_For_Services;

namespace Webshop.Services.Services.Validators.Implementations
{
    public class PasswordValidator : IValidationManager
    {
        public string IsAvailable(string key)
        {
            return "true";
        }
    }
}
