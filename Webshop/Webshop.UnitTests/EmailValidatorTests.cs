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
    public class EmailValidatorTests
    {
        private Mock<IUserManager> _mockUserManager;
        private EmailValidator _emailValidator;

        [SetUp]
        public void Setup()
        {
            _mockUserManager = new Mock<IUserManager>();
            _emailValidator = new EmailValidator(_mockUserManager.Object);
        }
        [Test]
        public void TestThatIsAvailableReturnsErrorMessageWhenEmailIsNull()
        {
            var result = _emailValidator.IsAvailable(null);

            Assert.IsNotNull(result);
            Assert.AreEqual("Hiba! Nem adtál meg email címet!", result);
        }
        [Test]
        public void TestThatIsAvailableReturnsErrorMessageWhenEmailNotContainsAtSymbol()
        {
            var result = _emailValidator.IsAvailable("testest");

            Assert.IsNotNull(result);
            Assert.AreEqual("Hiba! Nem érvényes email cím!", result);
        }
        [Test]
        public void TestThatIsAvailableReturnsErrorMessageWhenEmailNotContainsDotHuOrDotCom()
        {
            var result = _emailValidator.IsAvailable("test@test");

            Assert.IsNotNull(result);
            Assert.AreEqual("Hiba! Nem érvényes email cím!", result);
        }
        [Test]
        public void TestThatIsAvailableReturnsErrorMessageWhenEmailIsUsed()
        {
            var user = new UserData
            {
                Id = 1,
                EmailAddress = "test@test.com"
            };
            _mockUserManager.Setup(x => x.GetUsers()).Returns(new List<UserData> { user }.AsQueryable());

            var result = _emailValidator.IsAvailable("test@test.com");

            Assert.IsNotNull(result);
            Assert.AreEqual("Hiba! Az email cím használatban van!", result);
        }
        [Test]
        public void TestThatIsAvailableReturns200WhenEmailIsCorrectAndNotUser()
        {
            var user = new UserData
            {
                Id = 1,
                EmailAddress = "test@test.com"
            };
            _mockUserManager.Setup(x => x.GetUsers()).Returns(new List<UserData> { user }.AsQueryable());
            var result = _emailValidator.IsAvailable("asd@asd.hu");

            Assert.IsNotNull(result);
            Assert.AreEqual("200", result);
        }
    }
}
