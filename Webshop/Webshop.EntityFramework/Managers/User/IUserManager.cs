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
        /// <param name="user">The user object which should be added to the database.</param>
        void AddUser(UserData user);
        /// <summary>
        /// This method requires an userId as parameter. It returns the user by id from the database.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>Returns an user object.</returns>
        UserData GetUser(int userId);

    }
}
