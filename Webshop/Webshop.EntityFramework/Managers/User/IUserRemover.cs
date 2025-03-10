using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.User
{
    /// <summary>
    /// This interface helps removing an user from the database
    /// </summary>
    public interface IUserRemover
    {
        /// <summary>
        /// This method requires an user as parameter. It removes the specific user from the database.
        /// </summary>
        /// <param name="user"></param>
        void RemoveUser(UserData user);
    }
}
