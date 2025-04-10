using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Webshop.Services.Interfaces;

namespace Webshop.Services.Services.Security
{
    /// <summary>
    /// This is the implementation for <see cref="IEncryptManager"/>.
    /// Provides method for handling hashing.
    /// </summary>>
    public class SHA256Encrypter : IEncryptManager
    {
        /// <summary>
        /// Hashes the given string using SHA256 and returns the result as a Base64-encoded string.
        /// </summary>
        /// <param name="key">The string to be hashed.</param>
        /// <returns>The Base64-encoded SHA256 hash of the input string.</returns>
        public string Hash(string key)
        {
            using (var sha = SHA256.Create())
            {
                byte[] keyInBytes = Encoding.UTF8.GetBytes(key);
                byte[] hashedKey = sha.ComputeHash(keyInBytes);
                return Convert.ToBase64String(hashedKey);

            }
        }
    }
}
