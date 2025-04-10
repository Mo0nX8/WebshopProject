using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.User
{
    /// <summary>
    /// This interface helps editing an user's data
    /// </summary>
    public interface IUserEditor
    {
        /// <summary>
        /// This method requires an user as parameter. It updates the specified user's data in the database.
        /// </summary>
        /// <param name="user">The user object which should be updated.</param>
        void UpdateUser(UserData user);
    }
}
