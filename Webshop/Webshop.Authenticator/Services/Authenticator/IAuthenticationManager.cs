using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Authenticator.Services.Authenticator
{
    public interface IAuthenticationManager
    {
        void TryLogin();
        void LogOut();
        bool IsAuthenticated {  get; }

    }
}
