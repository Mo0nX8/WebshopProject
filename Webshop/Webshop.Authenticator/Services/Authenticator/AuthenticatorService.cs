using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Managers.Interfaces.User;
using Webshop.Services.Interfaces_For_Services;

namespace Webshop.Authenticator.Services.Authenticator
{
    
    public class AuthenticatorService : IAuthenticationManager
    {
        private IHttpContextAccessor httpContextAccessor; 
        private IUserManager userManager;
        private IEncryptManager encryptManager;

        public AuthenticatorService(IHttpContextAccessor httpContextAccessor, IEncryptManager encryptManager, IUserManager userManager)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.encryptManager = encryptManager;
            this.userManager = userManager;
        }

        public bool IsAuthenticated =>
            httpContextAccessor.HttpContext.Session.TryGetValue("email", out byte[] value);

        public void LogOut()
        {
            httpContextAccessor.HttpContext.Session.Clear();
        }

        public bool TryLogin(string email, string password)
        {
            var userInDatabase=userManager.GetUsers().FirstOrDefault(user=>user.EmailAddress == email);
            if (userInDatabase == null) 
            {
                return false;
            }
            string passwordHashed = encryptManager.Hash(password);
            if (passwordHashed != userInDatabase.Password)
            {
                return false;
            }
            if (httpContextAccessor.HttpContext.Session.TryGetValue("email", out byte[] value)) 
            {
                return false;
            }
            return true;
        }
    }
}
