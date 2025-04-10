using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.User
{
    /// <summary>
    /// This is the implementation for the IUserManager interface.
    /// Provides methods for managing user data, such as adding, removing, updating, and retrieving users.
    /// </summary>
    public class UserManager : IUserManager
    {
        private IUserRepository _userRepository;

        /// <summary>
        /// Constructor that initializes the UserManager with a specified IUserRepository.
        /// </summary>
        /// <param name="userRepository">The repository used to interact with user data.</param>
        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Adds a new user to the repository.
        /// </summary>
        /// <param name="user">The user to be added.</param>
        public void AddUser(UserData user)
        {
            if (user is not null)
            {
                _userRepository.AddUser(user);
            }
        }

        /// <summary>
        /// Removes a user from the repository.
        /// </summary>
        /// <param name="user">The user to be removed.</param>
        public void RemoveUser(UserData user)
        {
            if (user is not null)
            {
                _userRepository.RemoveUser(user);
            }
        }

        /// <summary>
        /// Retrieves all users from the repository.
        /// </summary>
        /// <returns>An IQueryable collection of users.</returns>
        public IQueryable<UserData> GetUsers()
        {
            return _userRepository.GetUsers();
        }

        /// <summary>
        /// Updates the details of an existing user in the repository.
        /// </summary>
        /// <param name="user">The user with updated data.</param>
        public void UpdateUser(UserData user)
        {
            if (user is not null)
            {
                _userRepository.UpdateUser(user);
            }
        }

        /// <summary>
        /// Retrieves a user from the repository by their unique ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>The user with the specified ID, or null if the user does not exist.</returns>
        public UserData GetUser(int userId)
        {
            if (userId != null)
            {
                return _userRepository.GetUser(userId);
            }
            return null;
        }
    }
}
