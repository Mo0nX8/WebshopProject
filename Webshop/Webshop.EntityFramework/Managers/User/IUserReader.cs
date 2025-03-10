using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.User
{
    /// <summary>
    /// This interface helps reading user's datas
    /// </summary>
    public interface IUserReader
    {
        /// <summary>
        /// This method returns all user's datas.
        /// </summary>
        /// <returns></returns>
        IQueryable<UserData> GetUsers();
    }
}
