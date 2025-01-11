using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Services.Interfaces_For_Services;

namespace Webshop.Authenticator.Services.Authenticator
{
    public class AuthenticatorService : IAuthenticationManager
    {
        public bool IsAuthenticated => throw new NotImplementedException();

        public void LogOut()
        {
            throw new NotImplementedException();
        }

        public void TryLogin()
        {
            throw new NotImplementedException();
        }
    }
}
