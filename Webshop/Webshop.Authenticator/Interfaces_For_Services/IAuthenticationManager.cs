using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Services.Interfaces_For_Services
{
    /// <summary>
    /// This interface helps with authentication.
    /// </summary>
    public interface IAuthenticationManager
    {
        /// <summary>
        /// This method requires an email and a password as parameter. 
        /// This method tries to log into the site. It checks if user exists and if correct parameters were given to the user.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool TryLogin(string email, string password);
        /// <summary>
        /// This method logs out the currently logged in user.
        /// </summary>
        void LogOut();
        /// <summary>
        /// This property checks if the user is authenticated or not.
        /// </summary>
        bool IsAuthenticated { get; set; }

    }
}
