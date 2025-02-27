using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Authenticator.Services.Encrypt;
using Webshop.Services.Interfaces_For_Services;

namespace Webshop.UnitTests
{
    public class SHA256EncrypterTests
    {
        private SHA256Encrypter _encrypter;
        [SetUp]
        public void Setup()
        {
            _encrypter = new SHA256Encrypter();
        }
        [TestCase("0123", "G+LkUrRteg2WVrux92joJI66G3W67WX12Z6vqUiJmmo=")]
        [TestCase("0000", "mvFbM25qlhmShTffMLLmojdlafz51+dz7M7eZWBlKaA=")]
        [TestCase("2412421", "meinqPadiWQMBuQhz2RuLcPMkzbho6NwUPdL/rAJY6A=")]
        [Test]
        public void TestThatEncriptionWorksWithOnlyNumbersProvided(string input, string expected)
        {
            var result=_encrypter.Hash(input);
            Assert.AreEqual(expected, result);
        }
        [TestCase("asd", "aIeH2P8UTFAsf1z/qv4sxYjYYHn53ogwTCawy5nOkcY=")]
        [TestCase("sziaalma", "W7YsqYsdi+vwEK5tgnqinmYYtI8YenxQrVv4NNeWsTA=")]
        [TestCase("IsThisGood", "O3s60cJmPVGLQpO02XOlS43pfDKUjXOwTsTefJ3cLFA=")]
        [Test]
        public void TestThatEncriptionWorksWithOnlyLettersProvided(string input, string expected)
        {
            var result = _encrypter.Hash(input);
            Assert.AreEqual(expected, result);
        }
        [Test]
        public void TestThatEncriptionWorksWithEmptyString()
        {
            string input=string.Empty;
            var res= _encrypter.Hash(input);
            string expected = "47DEQpj8HBSa+/TImW+5JCeuQeRkm5NMpJWZG3hSuFU=";
            Assert.AreEqual(expected, res);
        }
        [Test]
        public void TestThatEncriptionNotThrowErrorOnLongString()
        {
            string input = new string('a', 1000);
            var res= _encrypter.Hash(input);
            Assert.DoesNotThrow(() => _encrypter.Hash(input));
        }
        [Test]
        public void TestThatEncriptionWorksWithSpecialCharactersInString()
        {
            string input = "!@#$%^&*()";
            var res=_encrypter.Hash(input);
            var expected = "lc54nFydGEkJcnCYOMo6lxkJS8o6wWMyz+wGUrAjYUE=";
            Assert.AreEqual(expected, res);
        }
        [Test]
        public void TestThatEncriptionIsConsistentWhenSameStringGettingHashedTwoTimesInARow()
        {
            string input = "ThisIsATestString";
            var res1= _encrypter.Hash(input);
            var res2=_encrypter.Hash(input);
            Assert.AreEqual(res2, res1);
        }
        [TestCase("Hello","World")]
        [TestCase("CSharp", "Programming")]
        [TestCase("Encryption", "Security")]
        [TestCase("Hashing", "Algorithm")]
        [TestCase("Tech", "Code")]
        [Test]
        public void TestThatWhenGivenTwoDifferentStringTheResultIsNotTheSame(string firstString, string secondString)
        {
            var hash1= _encrypter.Hash(firstString);
            var hash2= _encrypter.Hash(secondString);
            Assert.AreNotEqual(hash1, hash2);
        }
        [Test]
        public void TestThatEncriptionGiveHashedCodeForOnlySpaceAsInput()
        {
            string input = " ";
            var res= _encrypter.Hash(input);
            string expected = "Nqnn8clbgv+5l0PgxcTOldg8mkMKrFn4TvPL+rYUUGg=";
            Assert.AreEqual(expected, res);
        }
        [Test]
        public void TestThatHashGivesShortHashOnOnlyOneCharacterGiven()
        {
            string input = "a";
            string hashed = _encrypter.Hash(input);
            Assert.AreEqual(44, hashed.Length);  

        }

    }
}
