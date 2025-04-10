using Microsoft.AspNetCore.Http;
using System.Text;
using Webshop.EntityFramework.Managers.User;
using Webshop.Services.Interfaces;

namespace Webshop.Services.Services.Authentication
{
    /// <summary>
    /// This is the implementation for IAuthenticationManager
    /// </summary>

    public class AuthenticationService : IAuthenticationManager
    {
        private IHttpContextAccessor httpContextAccessor;
        private IUserManager userManager;
        private IEncryptManager encryptManager;

        public AuthenticationService(IHttpContextAccessor httpContextAccessor, IEncryptManager encryptManager, IUserManager userManager)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.encryptManager = encryptManager;
            this.userManager = userManager;
        }

        public bool IsAuthenticated
        {
            get
            {
                var session = httpContextAccessor.HttpContext?.Session;
                if (session == null)
                {
                    return false;
                }

                session.TryGetValue("IsAuthenticated", out var valueBytes);
                var isAuthenticated = valueBytes != null && Encoding.UTF8.GetString(valueBytes) == "true";
                Console.WriteLine($"IsAuthenticated getter: {isAuthenticated}");
                return isAuthenticated;
            }
            set
            {
                var session = httpContextAccessor.HttpContext?.Session;
                if (session != null)
                {
                    var valueBytes = Encoding.UTF8.GetBytes(value ? "true" : "false");
                    session.Set("IsAuthenticated", valueBytes);
                    Console.WriteLine($"IsAuthenticated setter: {value}");
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
            var userInDatabase = userManager.GetUsers().FirstOrDefault(user => user.EmailAddress == email);
            if (userInDatabase == null)
            {
                return false;
            }

            string passwordHashed = encryptManager.Hash(password);
            if (passwordHashed != userInDatabase.Password)
            {
                return false;
            }


            IsAuthenticated = true;

            return true;
        }


    }
}
