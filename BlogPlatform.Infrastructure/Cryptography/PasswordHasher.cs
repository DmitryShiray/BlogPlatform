using System;
using System.Security.Cryptography;

namespace BlogPlatform.Infrastructure.Cryptography
{
    public class PasswordHasher
    {
        public byte[] GetPassword(string password, byte[] salt = null)
        {
            if (String.IsNullOrEmpty(password)) return null;

            if (null == salt)
            {
                salt = new byte[8];
                (RandomNumberGenerator.Create()).GetBytes(salt);
            }

            return Cryptographer.GenerateHash(password, salt);
        }

        public byte[] GetRandomSalt()
        {
            var salt = new byte[8];
            (RandomNumberGenerator.Create()).GetBytes(salt);

            return salt;
        }
    }
}
