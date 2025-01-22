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

        public bool IsAuthenticated
        {
            get
            {
                var session =httpContextAccessor.HttpContext?.Session;
                if (session == null)
                {
                    return false;
                }

                session.TryGetValue("IsAuthenticated", out var valueBytes);
                return valueBytes != null && System.Text.Encoding.UTF8.GetString(valueBytes) == "true";
            }
            set
            {
                var session = httpContextAccessor.HttpContext?.Session;
                if (session != null)
                {
                    var valueBytes = System.Text.Encoding.UTF8.GetBytes(value ? "true" : "false");
                    session.Set("IsAuthenticated", valueBytes);
                }
            }
        }

        public void LogOut()
        {
            IsAuthenticated = false;
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
            IsAuthenticated = true;
            return true;
        }
    }
}
