using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Authenticator.Services.Authenticator;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Interfaces.User;
using Webshop.Services.Interfaces_For_Services;

namespace Webshop.UnitTests
{
    [TestFixture]
    public class AuthenticatorServiceTests
    {
        private Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private Mock<IUserManager> _mockUserManager;
        private Mock<IEncryptManager> _mockEncryptManager;
        private AuthenticatorService _service;

        [SetUp]
        public void Setup()
        {
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockUserManager = new Mock<IUserManager>();
            _mockEncryptManager = new Mock<IEncryptManager>();

            var sessionMock= new Mock<ISession>();
            _mockHttpContextAccessor.Setup(x=>x.HttpContext.Session).Returns(sessionMock.Object);

            _service = new AuthenticatorService(
                _mockHttpContextAccessor.Object,
                _mockEncryptManager.Object,
                _mockUserManager.Object
                );
        }
        [Test]
        public void TestThatIsAuthenticatedReturnsTrueWhenSessonValueIsTrue()
        {
            var mockSession = new Mock<ISession>();
            _mockHttpContextAccessor.Setup(x => x.HttpContext.Session).Returns(mockSession.Object);

            var valueBytes = Encoding.UTF8.GetBytes("true");
            mockSession.Setup(s => s.TryGetValue("IsAuthenticated", out valueBytes)).Returns(true);

            var result = _service.IsAuthenticated;

            Assert.IsTrue(result);
        }
        [Test]
        public void TestThatAuthenticatedSetsSessionCorrectly()
        {
            var mockSession= new Mock<ISession>();
            _mockHttpContextAccessor.Setup(x => x.HttpContext.Session).Returns(mockSession.Object);

            _service.IsAuthenticated = true;

            mockSession.Verify(x => x.Set("IsAuthenticated", It.Is<byte[]>(b=> Encoding.UTF8.GetString(b) == "true")),Times.Once());
        }
        [Test]
        public void TestThatLogOutClearsSessionAndSetsIsAuthenticatedToFalse()
        {
            var mockSession = new Mock<ISession>();
            _mockHttpContextAccessor.Setup(x => x.HttpContext.Session).Returns(mockSession.Object);

            _service.LogOut();

            Assert.IsFalse(_service.IsAuthenticated);
            mockSession.Verify(x=>x.Clear(),Times.Once);
        }
        [Test]
        public void TestThatTryLoginReturnsFalseWhenUserDoesNotExist()
        {
            var userEmail = "test@gmail.com";
            var userPassword = "asd123";

            var mockUsers = new List<UserData>().AsQueryable();
            _mockUserManager.Setup(x=>x.GetUsers()).Returns(mockUsers);

            var result=_service.TryLogin(userEmail, userPassword);

            Assert.IsFalse(result);
        }
        [Test]
        public void TestThatTryLoginReturnsFalseWhenUserPasswordIsIncorrect()
        {
            var userEmail = "test@gmail.com";
            var userPassword = "asd123";
            var storedPasswordHash = "hashedasd123";

            var user = new UserData
            {
                EmailAddress = userEmail,
                Password = storedPasswordHash,
            };

            _mockUserManager.Setup(x => x.GetUsers()).Returns(new List<UserData> { user}.AsQueryable());
            _mockEncryptManager.Setup(x => x.Hash(It.IsAny<string>())).Returns("IncorrectHashedPassword");

            var result= _service.TryLogin(userEmail,userPassword);
            Assert.IsFalse(result);

        }
        [Test]
        public void TestThatTryLoginReturnsTrueSetsIsAuthenticatedInSessionTrueWhenCredentialsAreGood()
        {
            var userEmail = "test@example.com";
            var userPassword = "password123";
            var storedPasswordHash = "hashedPassword123";

            var user = new UserData { EmailAddress = userEmail, Password = storedPasswordHash };
            _mockUserManager.Setup(x => x.GetUsers()).Returns(new List<UserData> { user }.AsQueryable());
            _mockEncryptManager.Setup(x => x.Hash(It.IsAny<string>())).Returns(storedPasswordHash);

            var mockSession = new Mock<ISession>();

            mockSession.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
                .Callback<string, byte[]>((key, value) => { });

            byte[] value = Encoding.UTF8.GetBytes("true");
            mockSession.Setup(x => x.TryGetValue(It.IsAny<string>(), out value))
                .Returns(true);

            _mockHttpContextAccessor.Setup(x => x.HttpContext.Session).Returns(mockSession.Object);

            var result = _service.TryLogin(userEmail, userPassword);

            Assert.IsTrue(result);
            Assert.IsTrue(_service.IsAuthenticated);



        }
    }
}
