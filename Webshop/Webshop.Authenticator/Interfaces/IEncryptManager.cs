using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Services.Interfaces
{
    /// <summary>
    /// This interface is for the encryption.
    /// </summary>
    public interface IEncryptManager
    {
        /// <summary>
        /// This method requires a string key as parameter. It returns a string, which is Hashed by SHA256 protocol.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string Hash(string key);
    }
}
