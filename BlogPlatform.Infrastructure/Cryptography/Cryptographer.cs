using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BlogPlatform.Infrastructure
{
    public static class Cryptographer
    {
        private static byte[] _key;
        private static byte[] _iv;

        public static void Initialize(byte[] key, byte[] iv)
        {
            _key = key;
            _iv = iv;
        }

        #region Hashing

        public static byte[] GenerateHash(string plainString, byte[] salt = null)
        {
            var hash = SHA256.Create();
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(plainString);
                byte[] dataWithSalt;

                if (null != salt)
                {
                    dataWithSalt = new byte[data.Length + salt.Length];

                    for (int i = 0; i < data.Length; i++)
                        dataWithSalt[i] = data[i];

                    for (int i = data.Length, j = 0; i < dataWithSalt.Length; i++, j++)
                        dataWithSalt[i] = salt[j];
                }
                else
                {
                    dataWithSalt = data;
                }

                return hash.ComputeHash(dataWithSalt);
            }
            finally
            {
                hash.Dispose();
            }
        }

        public static void GenerateKey(out byte[] key, out byte[] initializationVector)
        {
            var keyProvider = Aes.Create();
            key = keyProvider.Key;
            initializationVector = keyProvider.IV;
            keyProvider.Dispose();
        }

        public static bool VerifyHash(string hash, byte[] data, string salt)
        {
            byte[] hashToVerify = Convert.FromBase64String(hash);
            byte[] saltToVerify = Convert.FromBase64String(salt);
            byte[] dataToVerify = data;
            return VerifyHash(dataToVerify, hashToVerify, saltToVerify);
        }

        private static bool VerifyHash(byte[] data, byte[] hash, byte[] salt)
        {
            var newHash = GenerateHash(Encoding.UTF8.GetString(data), salt);
            return newHash.Length == hash.Length && hash.SequenceEqual(newHash);
        }

        public static string GetHashString(string data, string salt)
        {
            byte[] hash = GenerateHash(data, Convert.FromBase64String(salt));
            return Convert.ToBase64String(hash);
        }

        #endregion

        #region Encryption

        public static byte[] Encrypt(byte[] data, byte[] key = null, byte[] iv = null)
        {
            var cryptographer = Aes.Create();
            var buffer = new MemoryStream();

            try
            {
                ICryptoTransform cryptor = cryptographer.CreateEncryptor(key ?? _key, iv ?? _iv);

                var cryptedBuffer = new CryptoStream(buffer, cryptor, CryptoStreamMode.Write);
                cryptedBuffer.Write(data, 0, data.Length);
                cryptedBuffer.Dispose();

                return buffer.ToArray();
            }
            finally
            {
                buffer.Dispose();
                cryptographer.Dispose();
            }
        }

        public static byte[] Encrypt(string plainString, byte[] key = null, byte[] iv = null)
        {
            return Encrypt(Encoding.UTF8.GetBytes(plainString), key, iv);
        }

        public static byte[] Decrypt(byte[] encryptedData, byte[] key = null, byte[] iv = null)
        {
            var cryptographer = Aes.Create();
            var buffer = new MemoryStream(encryptedData);
            CryptoStream cryptedBuffer = null;

            try
            {
                ICryptoTransform decryptor = cryptographer.CreateDecryptor(key ?? _key, iv ?? _iv);
                cryptedBuffer = new CryptoStream(buffer, decryptor, CryptoStreamMode.Read);

                return cryptedBuffer.ReadToEnd();
            }
            finally
            {
                if (null != cryptedBuffer)
                    cryptedBuffer.Dispose();
                buffer.Dispose();
                cryptographer.Dispose();
            }
        }

        public static string Decrypt(string encryptedValue,
                                     bool isBase64String = true,
                                     byte[] key = null,
                                     byte[] iv = null)
        {
            byte[] encryptedData;
            if (isBase64String)
            {
                encryptedData = Convert.FromBase64String(encryptedValue);
            }
            else
            {
                int numChars = encryptedValue.Length;
                encryptedData = new byte[numChars / 2];
                for (int i = 0; i < numChars; i += 2)
                    encryptedData[i / 2] = Convert.ToByte(encryptedValue.Substring(i, 2), 16);
            }

            byte[] rawData = Decrypt(encryptedData, key, iv);
            return Encoding.UTF8.GetString(rawData);
        }

        #endregion
    }
}
