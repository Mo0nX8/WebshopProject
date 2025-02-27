using Microsoft.EntityFrameworkCore;
using System.Linq;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Interfaces.User;

namespace Webshop.EntityFramework.Managers.Implementations
{
    /// <summary>
    /// This is the implementation for IUserManager.
    /// </summary>
    public class UserManager : IUserManager
    {
        private readonly GlobalDbContext _context;

        public UserManager(GlobalDbContext context)
        {
            _context = context;
        }

        public void AddUser(UserData user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void RemoveUser(UserData user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public IQueryable<UserData> GetUsers()
        {
            return _context.Users.AsQueryable();
        }

        public void UpdateUser(UserData user)
        {
            _context.Update(user);
            _context.SaveChanges();
        }

        public UserData GetUser(int userId)
        {
            return _context.Users.FirstOrDefault(x => x.Id == userId);
        }
       
    }
}
