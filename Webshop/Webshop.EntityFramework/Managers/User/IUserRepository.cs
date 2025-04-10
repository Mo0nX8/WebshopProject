using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.User
{
    /// <summary>
    /// Interface for managing user data in the repository.
    /// Defines methods for adding, updating, removing, and retrieving users.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Removes a user from the repository.
        /// </summary>
        /// <param name="user">The user to be removed.</param>
        void RemoveUser(UserData user);

        /// <summary>
        /// Retrieves all users from the repository.
        /// </summary>
        /// <returns>An IQueryable collection of all users.</returns>
        IQueryable<UserData> GetUsers();

        /// <summary>
        /// Retrieves a specific user by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve.</param>
        /// <returns>The user with the specified ID.</returns>
        UserData GetUser(int userId);

        /// <summary>
        /// Adds a new user to the repository.
        /// </summary>
        /// <param name="user">The user to be added.</param>
        void AddUser(UserData user);

        /// <summary>
        /// Updates an existing user's data in the repository.
        /// </summary>
        /// <param name="user">The user with updated data.</param>
        void UpdateUser(UserData user);
    }
}
