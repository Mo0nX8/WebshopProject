using Microsoft.EntityFrameworkCore;
using System.Linq;
using Webshop.EntityFramework.Data;

namespace Webshop.EntityFramework.Managers.User
{
    /// <summary>
    /// This is the implementation for IUserManager.
    /// </summary>
    public class UserManager : IUserManager
    {
        private IUserRepository _userRepository;

        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void AddUser(UserData user)
        {
            if (user is not null)
            {
                _userRepository.AddUser(user);
            }
        }

        public void RemoveUser(UserData user)
        {
            if (user is not null)
            {
                _userRepository.RemoveUser(user);
            }
        }

        public IQueryable<UserData> GetUsers()
        {
            return _userRepository.GetUsers();
        }

        public void UpdateUser(UserData user)
        {
            if (user is not null)
            {
                _userRepository.UpdateUser(user);
            }
        }

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
