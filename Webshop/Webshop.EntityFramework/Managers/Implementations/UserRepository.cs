using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Interfaces.User;

namespace Webshop.EntityFramework.Managers.Implementations
{
    public class UserRepository : IUserRepository
    {
        private GlobalDbContext _context;

        public UserRepository(GlobalDbContext context)
        {
            _context = context;
        }

        public void AddUser(UserData user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public UserData GetUser(int userId)
        {
            return _context.Users.FirstOrDefault(x => x.Id == userId);
        }

        public IQueryable<UserData> GetUsers()
        {
            return _context.Users.AsQueryable();
        }

        public void RemoveUser(UserData user)
        {

            _context.Users.Remove(user);
            _context.SaveChanges();

        }

        public void UpdateUser(UserData user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
