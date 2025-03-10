using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.User;

namespace Webshop.UnitTests
{
    [TestFixture]
    public class UserManagerTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private UserManager _userManager;

        [SetUp]
        public void Setup()
        {
            _userRepositoryMock=new Mock<IUserRepository>();
            _userManager = new UserManager(_userRepositoryMock.Object);
        }
        [Test]
        public void TestThatAddUserGetsCalledWhenUserIsValid()
        {
            var user = new UserData { Id = 1, Username = "Moon" };

            _userRepositoryMock.Setup(x => x.AddUser(It.IsAny<UserData>())).Verifiable();

            _userManager.AddUser(user);

            _userRepositoryMock.Verify(x=> x.AddUser(It.Is<UserData>(u=>u==user)),Times.Once);    
        }
        [Test]
        public void TestThatAddUserNotGettingCalledWhenUserIsNull()
        {
            _userRepositoryMock.Setup(x => x.AddUser(It.IsAny<UserData>())).Verifiable();

            _userManager.AddUser(null);

            _userRepositoryMock.Verify(x => x.AddUser(It.IsAny<UserData>()), Times.Never);
        }
        [Test]
        public void TestThatRemoveUserGetsCalledWhenUserIsValid()
        {
            var user = new UserData { Id = 1, Username = "Moon" };

            _userRepositoryMock.Setup(x => x.RemoveUser(It.IsAny<UserData>())).Verifiable();

            _userManager.RemoveUser(user);

            _userRepositoryMock.Verify(x => x.RemoveUser(It.Is<UserData>(u => u == user)), Times.Once);
        }
        [Test]
        public void TestThatRemoveUserNotGettingCalledWhenUserIsNull()
        {
            _userRepositoryMock.Setup(x => x.RemoveUser(It.IsAny<UserData>())).Verifiable();

            _userManager.AddUser(null);

            _userRepositoryMock.Verify(x => x.RemoveUser(It.IsAny<UserData>()), Times.Never);
        }
        [Test]
        public void TestThatGetUsersReturnsUsersAsQueryable()
        {
            var users= new List<UserData>
            {
                new UserData {Id=1, Username="John Doe"},
                new UserData {Id=2, Username="Mo0nX8"},
            };

            _userRepositoryMock.Setup(x=>x.GetUsers()).Returns(users.AsQueryable());

            var result = _userManager.GetUsers(); ;

            Assert.AreEqual(users.Count(), result.Count());
            Assert.Contains(users[0], result.ToList());
            Assert.Contains(users[1], result.ToList());
        }
        [Test]
        public void TestThatGetUsersWhenNoUserReturnsEmptyList()
        {
            _userRepositoryMock.Setup(x => x.GetUsers()).Returns(new List<UserData>().AsQueryable());

            var result = _userManager.GetUsers();

            Assert.IsEmpty(result);
        }
        [Test]
        public void TestThatUpdateUserIsCalledWhenUserIsValid()
        {
            var user = new UserData { Id = 1, Username = "Moon" };

            _userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<UserData>())).Verifiable();

            _userManager.UpdateUser(user);

            _userRepositoryMock.Verify(x => x.UpdateUser(It.Is<UserData>(u => u == user)), Times.Once);
        }
        [Test]
        public void TestThatUpdateUserNotGettingCalledWhenUserIsInvalid()
        {
            _userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<UserData>())).Verifiable();

            _userManager.UpdateUser(null);

            _userRepositoryMock.Verify(x => x.UpdateUser(It.IsAny<UserData>()), Times.Never);
        }
        [Test]
        public void TestThatGetUserReturnsUserWhenUserIdIsValid()
        {
            var userId = 1;
            var user = new UserData { Id = 1, Username = "John Doe" };
            _userRepositoryMock.Setup(repo => repo.GetUser(userId)).Returns(user);

            var result = _userManager.GetUser(userId);

            Assert.AreEqual(user, result);
        }
        [Test]
        public void TestThatGetUserReturnsNullWhenUserIdIsInvalid()
        {
            var userid = 9999;
            _userRepositoryMock.Setup(x => x.GetUser(1)).Returns((UserData)null);

            var result = _userManager.GetUser(userid);

            Assert.IsNull(result);
        }
        [Test]
        public void TestThatGetUserReturnsNullWhenUserIdIsNull()
        {
            int? userId = null;

            var result = _userManager.GetUser(userId ?? 0);
            Assert.IsNull(result);
        }

    }

}
