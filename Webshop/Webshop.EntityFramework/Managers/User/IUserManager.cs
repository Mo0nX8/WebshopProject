using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.User
{
    /// <summary>
    /// This interface helps managing users.
    /// </summary>
    public interface IUserManager : IUserRepository
    {
        /// <summary>
        /// This method requires an user as parameter. It adds the user to the database.
        /// </summary>
        /// <param name="user"></param>
        void AddUser(UserData user);
        /// <summary>
        /// This method requires an userId as parameter. It returns the user by id from the database.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserData GetUser(int userId);

    }
}
