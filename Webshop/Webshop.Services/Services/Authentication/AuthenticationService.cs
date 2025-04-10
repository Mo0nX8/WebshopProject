using Microsoft.AspNetCore.Http;
using System.Text;
using Webshop.EntityFramework.Managers.User;
using Webshop.Services.Interfaces;

namespace Webshop.Services.Services.Authentication
{
    /// <summary>
    /// This is the implementation for <see cref="IAuthenticationManager"/>.
    /// Provides methods for handling user authentication through session-based state.
    /// </summary>>

    public class AuthenticationService : IAuthenticationManager
    {
        private IHttpContextAccessor httpContextAccessor;
        private IUserManager userManager;
        private IEncryptManager encryptManager;
        /// <summary>
        /// Constructor for initializing <see cref="AuthenticationService"/> with dependencies.
        /// </summary>
        /// <param name="httpContextAccessor">Provides access to the current HTTP context.</param>
        /// <param name="encryptManager">Service for hashing and encrypting data.</param>
        /// <param name="userManager">Service to retrieve user information from the database.</param>
        public AuthenticationService(IHttpContextAccessor httpContextAccessor, IEncryptManager encryptManager, IUserManager userManager)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.encryptManager = encryptManager;
            this.userManager = userManager;
        }
        /// <summary>
        /// Gets or sets the authentication status of the current session.
        /// </summary>
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
                return isAuthenticated;
            }
            set
            {
                var session = httpContextAccessor.HttpContext?.Session;
                if (session != null)
                {
                    var valueBytes = Encoding.UTF8.GetBytes(value ? "true" : "false");
                    session.Set("IsAuthenticated", valueBytes);
                }
            }
        }

        /// <summary>
        /// Logs out the current user by clearing the authentication flag and session data.
        /// </summary>
        public void LogOut()
        {
            IsAuthenticated = false;
            httpContextAccessor.HttpContext.Session.Clear();
        }
        /// <summary>
        /// Attempts to log in the user by validating email and password.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <param name="password">The plaintext password to validate.</param>
        /// <returns>
        /// <c>true</c> if the credentials are valid and the user is logged in; otherwise, <c>false</c>.
        /// </returns>
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
