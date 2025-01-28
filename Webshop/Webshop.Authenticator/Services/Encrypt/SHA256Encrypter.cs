using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Webshop.Services.Interfaces_For_Services;

namespace Webshop.Authenticator.Services.Encrypt
{
    public class SHA256Encrypter : IEncryptManager
    {
        public string Hash(string key)
        {
            using(var sha=SHA256.Create())
            {
                byte[] keyInBytes=Encoding.UTF8.GetBytes(key);
                byte[] hashedKey= sha.ComputeHash(keyInBytes);
                return Convert.ToBase64String(hashedKey);
                ;
            }
        }
    }
}
