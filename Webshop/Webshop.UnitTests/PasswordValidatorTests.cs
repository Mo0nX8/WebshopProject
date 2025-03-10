using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Services.Services.Validators;

namespace Webshop.UnitTests
{
    [TestFixture]
    public class PasswordValidatorTests
    {
        private PasswordValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new PasswordValidator();
        }
        [Test]
        public void TestThatPasswordIsNullIsAvailableReturnsErrorMessage()
        {
            var result = validator.IsAvailable(null);
            Assert.AreEqual("Hiba! Nem adtál meg jelszót!", result);
        }
        [TestCase("password")]
        [TestCase("apple")]
        [TestCase("GoodPassowrd")]
        [TestCase("Asdasd")]
        [TestCase("Bottyanxd")]
        [Test]
        public void TestThatPasswordNotContainsAnyNumberIsAvailableReturnsErrorMessage(string key)
        {
            var result = validator.IsAvailable(key);
            Assert.AreEqual("Hiba! A jelszónak tartalmaznia kell legaláb egy számot!", result);
        }
        [Test]
        public void TestThatPasswordLengthIsBelowMinimumWhichIs8IsAvailableReturnsErrorMessage()
        {
            var result = validator.IsAvailable("little1");
            Assert.AreEqual("Hiba! A jelszónak legalább 8 karakter hosszúnak kell lennie!", result);
        }
        [Test]
        public void TestThatPasswordIsCorrectIsAvailableReturns200()
        {
            var result = validator.IsAvailable("GoodPassword69");
            Assert.AreEqual("200", result);
        }
    }
}
