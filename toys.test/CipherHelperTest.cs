using NUnit.Framework;
using toys.Helpers;

namespace toys.test
{
    [TestFixture]
    public class CipherHelperTest
    {
        /*
        [Test]
        public void TestEncryptDecript()
        {
            var s = "Hello World";
            var encrypt = CipherHelper.Encrypt(s);
            var decrypt = CipherHelper.Decrypt(encrypt);

            Assert.AreEqual(s, decrypt);
        }

        [Test]        
        public void TestEncryptDecript_ThrowException()
        {
            var s = "Hello World";
            var encrypt = CipherHelper.Encrypt(s) + "xxx";
            var decrypt = CipherHelper.Decrypt(encrypt);
            
            Assert.AreNotEqual(s, decrypt);
        }
        */

        [Test]
        public void TestEncryptDecriptUsingPassword()
        {
            var s = "Hello World";
            var pw = "qwerty123456";
            var encrypt = CipherHelper.Encrypt(s,pw);
            var decrypt = CipherHelper.Decrypt(encrypt,pw);

            Assert.AreEqual(s, decrypt);
        }

        [Test]
        public void TestEncryptDecriptUsingPassword_ThrowException()
        {
            var s = "Hello World";
            var pw = "qwerty123456";
            var encrypt = CipherHelper.Encrypt(s, pw) + "asd";
            var decrypt = CipherHelper.Decrypt(encrypt, pw);

            Assert.AreNotEqual(s, decrypt);
            Assert.IsEmpty(decrypt);
        }

        [Test]
        public void TestEncryptDecriptUsingEmptyPassword()
        {
            var s = "Hello World";
            var pw = "";
            var encrypt = CipherHelper.Encrypt(s, pw);
            var decrypt = CipherHelper.Decrypt(encrypt, pw);

            Assert.AreEqual(s, decrypt);
            // Assert.IsEmpty(decrypt);
        }

        [Test]
        public void TestAesEncryptDecrypt()
        {
            var s = "Hello World";
            var pw = "123456@123";
            var encrypt = CipherHelper.AesEncrypt(s, pw);
            var decrypt = CipherHelper.AesDecrypt(encrypt, pw);

            Assert.AreEqual(s, decrypt);
        }

        [Test]
        public void TestAesEncryptDecrypt_WithoutPassword()
        {
            var s = "Hello World";
            var pw = "";
            var encrypt = CipherHelper.AesEncrypt(s, pw);
            var decrypt = CipherHelper.AesDecrypt(encrypt, pw);

            Assert.AreEqual(s, decrypt);
        }

        [Test]
        public void TestAesEncryptDecrypt_ThrowException()
        {
            var s = "Hello World";
            var pw = "123456@123";
            var encrypt = CipherHelper.AesEncrypt(s, pw) + "asd";
            var decrypt = CipherHelper.AesDecrypt(encrypt, pw);

            Assert.AreEqual(s, decrypt);
        }

        /*
        [Test]
        public void TestCrossEncryptDecript()
        {
            var s = "Hello World";
            var pw = "";
            var encrypt = CipherHelper.Encrypt(s, pw);
            var decrypt = CipherHelper.Decrypt(encrypt);

            Assert.AreEqual(s, decrypt);
            // Assert.IsEmpty(decrypt);
        }
        */
    }
}
