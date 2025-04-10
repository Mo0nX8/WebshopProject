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
        /// <param name="user">The user object which should be removed.</param>
        void RemoveUser(UserData user);
    }
}
