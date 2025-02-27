using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Interfaces.User
{
    /// <summary>
    /// This interface helps editing an user's data
    /// </summary>
    public interface IUserEditor
    {
        /// <summary>
        /// This method requires an user as parameter. It updates the specified user's data in the database.
        /// </summary>
        /// <param name="user"></param>
        void UpdateUser(UserData user);
    }
}
