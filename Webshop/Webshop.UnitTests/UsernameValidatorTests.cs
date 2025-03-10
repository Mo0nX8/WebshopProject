using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.User;
using Webshop.Services.Services.Validators;

namespace Webshop.UnitTests
{
    [TestFixture]
    public class UsernameValidatorTests
    {
        private Mock<IUserManager> _mockUserManager;
        private UsernameValidator _usernameValidator;

        [SetUp]
        public void Setup()
        {
            _mockUserManager = new Mock<IUserManager>();
            _usernameValidator = new UsernameValidator(_mockUserManager.Object);
        }

        [Test]
        public void TestThatWhenUsernameIsNullIsAvailableReturnsErrorMessage()
        {
            var result = _usernameValidator.IsAvailable(null);

            Assert.AreEqual("Hiba! Nem adtál meg felhasználónevet!", result);
        }
        [Test]
        public void TestThatUsernameIsTakenIsAvailableReturnsErrorMessage()
        {
            var user = new UserData
            {
                Id = 1,
                Username = "Moon"
            };
            _mockUserManager.Setup(x=>x.GetUsers()).Returns(new List<UserData> { user}.AsQueryable());

            var result = _usernameValidator.IsAvailable("Moon");
            Assert.IsNotNull(result);
            Assert.AreEqual("Hiba! A felhasználónév már regisztrálva van.", result);
        }
        [Test]
        public void TestThatUsernameLengthIsBelowTheMinimumWhichIs6()
        {
            var result = _usernameValidator.IsAvailable("Moon");
            Assert.IsNotNull(result);
            Assert.AreEqual("Hiba! A felhasználónév nem lehet rövidebb 6 karakternél.", result);
        }
        [Test]
        public void TestThatUsernameIsValidIsAvailableReturns200()
        {
            var result = _usernameValidator.IsAvailable("GoodUsername");
            Assert.IsNotNull(result);
            Assert.AreEqual("200", result);
        }

    }
}
