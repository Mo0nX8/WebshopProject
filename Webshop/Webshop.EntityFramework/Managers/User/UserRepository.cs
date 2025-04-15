using Microsoft.EntityFrameworkCore;
using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.User
{
    /// <summary>
    /// This is the implementation for the IUserRepository interface.
    /// Provides methods for interacting with user data in the database, including adding, removing, updating, and retrieving users.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private GlobalDbContext _context;

        /// <summary>
        /// Constructor that initializes the UserRepository with the provided database context.
        /// </summary>
        /// <param name="context">The DbContext used to interact with the database.</param>
        public UserRepository(GlobalDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <param name="user">The user to be added.</param>
        public void AddUser(UserData user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        /// <summary>
        /// Retrieves a user from the database by their unique ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>The user with the specified ID, or null if no user is found.</returns>
        public UserData GetUser(int userId)
        {
            return _context.Users
                .Include(p => p.Address)
                .Include(p=>p.Cart)
                .ThenInclude(p=>p.CartItems)
                .ThenInclude(p=>p.Product)
                .FirstOrDefault(x => x.Id == userId);
        }

        /// <summary>
        /// Retrieves all users from the database.
        /// </summary>
        /// <returns>An IQueryable collection of users.</returns>
        public IQueryable<UserData> GetUsers()
        {
            return _context.Users.AsQueryable();
        }

        /// <summary>
        /// Removes a user from the database.
        /// </summary>
        /// <param name="user">The user to be removed.</param>
        public void RemoveUser(UserData user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        /// <summary>
        /// Updates the details of an existing user in the database.
        /// </summary>
        /// <param name="user">The user with updated details.</param>
        public void UpdateUser(UserData user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
