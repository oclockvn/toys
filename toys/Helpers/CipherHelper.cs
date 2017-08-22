using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace toys.Helpers
{
    internal static class Global
    {
        // set permutations
        public const string strPermutation = "qwertyuiopasdfgh";
        public const int bytePermutation1 = 0x19;
        public const int bytePermutation2 = 0x59;
        public const int bytePermutation3 = 0x17;
        public const int bytePermutation4 = 0x41;
    }

    public static class CipherHelper
    {
        #region without password
        /*
        /// <summary>
        /// Encrypt a string without password
        /// </summary>
        /// <param name="plainText">The string to be encrypted</param>
        /// <returns>Encrypted string</returns>
        public static string Encrypt(string plainText)
        {
            // https://msdn.microsoft.com/en-us/library/ds4kkd55(v=vs.110).aspx
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(plainText)));
        }
        
        /// <summary>
        /// Decrypt an encrypted string
        /// </summary>
        /// <param name="plainText">The string to be decrypted</param>
        /// <returns>The plain text string</returns>
        public static string Decrypt(string plainText)
        {
            // https://msdn.microsoft.com/en-us/library/system.convert.frombase64string(v=vs.110).aspx
            try
            {
                return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(plainText)));
            }
            catch (FormatException)
            {
                return string.Empty;
            }
            catch (CryptographicException)
            {
                return string.Empty;
            }
        }

        // encrypt
        public static byte[] Encrypt(byte[] strData)
        {
            PasswordDeriveBytes passbytes =
            new PasswordDeriveBytes(Global.strPermutation,
            new byte[] { Global.bytePermutation1,
                         Global.bytePermutation2,
                         Global.bytePermutation3,
                         Global.bytePermutation4
            });

            MemoryStream memstream = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = passbytes.GetBytes(aes.KeySize / 8);
            aes.IV = passbytes.GetBytes(aes.BlockSize / 8);

            CryptoStream cryptostream = new CryptoStream(memstream,
            aes.CreateEncryptor(), CryptoStreamMode.Write);
            cryptostream.Write(strData, 0, strData.Length);
            cryptostream.Close();
            return memstream.ToArray();
        }

        // decrypt
        public static byte[] Decrypt(byte[] strData)
        {
            PasswordDeriveBytes passbytes =
            new PasswordDeriveBytes(Global.strPermutation,
            new byte[] { Global.bytePermutation1,
                         Global.bytePermutation2,
                         Global.bytePermutation3,
                         Global.bytePermutation4
            });

            MemoryStream memstream = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = passbytes.GetBytes(aes.KeySize / 8);
            aes.IV = passbytes.GetBytes(aes.BlockSize / 8);

            CryptoStream cryptostream = new CryptoStream(memstream,
            aes.CreateDecryptor(), CryptoStreamMode.Write);
            cryptostream.Write(strData, 0, strData.Length);
            cryptostream.Close();
            return memstream.ToArray();
        }
        */
        #endregion

        #region Rijndael
        /// <summary>
        /// Encrypt a string with password using Rijndael.
        /// </summary>
        /// <param name="plainText">String to be encrypted</param>
        /// <param name="password">Password</param>
        public static string Encrypt(string plainText, string password)
        {
            if (string.IsNullOrEmpty(plainText) || string.IsNullOrWhiteSpace(plainText))
                return string.Empty;

            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password))
                password = string.Empty;

            // Get the bytes of the string
            var bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            var bytesEncrypted = Encrypt(bytesToBeEncrypted, passwordBytes);

            return Convert.ToBase64String(bytesEncrypted);
        }

        /// <summary>
        /// Decrypt a string with password using Rijndael.
        /// </summary>
        /// <param name="encryptedText">String to be decrypted</param>
        /// <param name="password">Password used during encryption</param>
        /// <exception cref="FormatException"></exception>
        public static string Decrypt(string encryptedText, string password)
        {
            if (string.IsNullOrEmpty(encryptedText) || string.IsNullOrWhiteSpace(encryptedText))
                return string.Empty;

            if (string.IsNullOrEmpty(password))
                password = string.Empty;

            try
            {
                // Get the bytes of the string
                var bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
                var passwordBytes = Encoding.UTF8.GetBytes(password);

                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                var bytesDecrypted = Decrypt(bytesToBeDecrypted, passwordBytes);

                return Encoding.UTF8.GetString(bytesDecrypted);
            }
            catch (FormatException)
            {
                return string.Empty;
            }
            catch (CryptographicException)
            {
                return string.Empty;
            }
        }

        private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }

                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        private static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }

                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }
        #endregion

        #region AES
        private static readonly int _saltSize = 32;

        /// <summary>
        /// Encrypts the plainText input using the given Key.
        /// A 128 bit random salt will be generated and prepended to the ciphertext before it is base64 encoded.
        /// </summary>
        /// <param name="plainText">The plain text to encrypt.</param>
        /// <param name="key">The plain text encryption key.</param>
        /// <returns>The salt and the ciphertext, Base64 encoded for convenience.</returns>
        public static string AesEncrypt(string plainText, string key)
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;

            if (string.IsNullOrEmpty(key))
                key = string.Empty;

            // Derive a new Salt and IV from the Key
            using (var keyDerivationFunction = new Rfc2898DeriveBytes(key, _saltSize))
            {
                var saltBytes = keyDerivationFunction.Salt;
                var keyBytes = keyDerivationFunction.GetBytes(32);
                var ivBytes = keyDerivationFunction.GetBytes(16);

                // Create an encryptor to perform the stream transform.
                // Create the streams used for encryption.
                using (var aesManaged = new AesManaged())
                using (var encryptor = aesManaged.CreateEncryptor(keyBytes, ivBytes))
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    using (var streamWriter = new StreamWriter(cryptoStream))
                    {
                        // Send the data through the StreamWriter, through the CryptoStream, to the underlying MemoryStream
                        streamWriter.Write(plainText);
                    }

                    // Return the encrypted bytes from the memory stream, in Base64 form so we can send it right to a database (if we want).
                    var cipherTextBytes = memoryStream.ToArray();
                    Array.Resize(ref saltBytes, saltBytes.Length + cipherTextBytes.Length);
                    Array.Copy(cipherTextBytes, 0, saltBytes, _saltSize, cipherTextBytes.Length);

                    return Convert.ToBase64String(saltBytes);
                }
            }
        }

        /// <summary>
        /// Decrypts the ciphertext using the Key.
        /// </summary>
        /// <param name="ciphertext">The ciphertext to decrypt.</param>
        /// <param name="key">The plain text encryption key.</param>
        /// <returns>The decrypted text.</returns>
        public static string AesDecrypt(string ciphertext, string key)
        {
            if (string.IsNullOrEmpty(ciphertext))
                return string.Empty;

            if (string.IsNullOrEmpty(key))
                key = string.Empty;

            try
            {
                // Extract the salt from our ciphertext
                var allTheBytes = Convert.FromBase64String(ciphertext);
                var saltBytes = allTheBytes.Take(_saltSize).ToArray();
                var ciphertextBytes = allTheBytes.Skip(_saltSize).Take(allTheBytes.Length - _saltSize).ToArray();

                using (var keyDerivationFunction = new Rfc2898DeriveBytes(key, saltBytes))
                {
                    // Derive the previous IV from the Key and Salt
                    var keyBytes = keyDerivationFunction.GetBytes(32);
                    var ivBytes = keyDerivationFunction.GetBytes(16);

                    // Create a decrytor to perform the stream transform.
                    // Create the streams used for decryption.
                    // The default Cipher Mode is CBC and the Padding is PKCS7 which are both good
                    using (var aesManaged = new AesManaged())
                    using (var decryptor = aesManaged.CreateDecryptor(keyBytes, ivBytes))
                    using (var memoryStream = new MemoryStream(ciphertextBytes))
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    using (var streamReader = new StreamReader(cryptoStream))
                    {
                        // Return the decrypted bytes from the decrypting stream.
                        return streamReader.ReadToEnd();
                    }
                }
            }
            catch (FormatException) { }
            catch (CryptographicException) { }

            return string.Empty;            
        }
        #endregion
    }
}
