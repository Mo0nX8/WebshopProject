using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Services.Interfaces_For_Services
{
    public interface IAuthenticationManager
    {
        bool TryLogin(string email, string password);
        void LogOut();
        bool IsAuthenticated { get; set; }

    }
}
