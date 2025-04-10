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
        /// <returns>Returns all user's as IQueryable type.</returns>
        IQueryable<UserData> GetUsers();
    }
}
