using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Authenticator.Services.Encrypt
{
    public interface IEncryptManager
    {
        string Hash(string key);
    }
}
