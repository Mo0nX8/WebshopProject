using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.Interfaces.User
{
    public interface IUserManager : IUserEditor, IUserReader, IUserRemover
    {
        void Add(UserData user);
        void Remove(UserData user);
        IQueryable<UserData> GetUsers();
        void Update(UserData user);
        UserData GetUser(int userId);
        
    }
}
