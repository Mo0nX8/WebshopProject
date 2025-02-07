using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Interfaces.User;

namespace Webshop.EntityFramework.Managers.Implementations
{

    public class UserManager : IUserManager
    {
        private GlobalDbContext _context;
        public UserManager(GlobalDbContext _context)
        {
            this._context = _context;
        }
        public void Add(UserData user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Remove(UserData user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public IQueryable<UserData> GetUsers()
        {
            return _context.Users.AsQueryable();
        }

        public void Update(UserData user)
        {
            _context.Update(user);
            _context.SaveChanges();
        }

        public UserData GetUser(int userId)
        {
            var user = _context.Users.FirstOrDefault(x=>x.Id==userId);
            return user;
        }
    }
}
