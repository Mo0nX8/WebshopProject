using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Interfaces.User
{
    public interface IUserRepository
    {
        void RemoveUser(UserData user);
        IQueryable<UserData> GetUsers();
        UserData GetUser(int userId);
        void AddUser(UserData user);
        void UpdateUser(UserData user);
    }
}
