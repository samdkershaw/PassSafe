using PassSafe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PassSafe.Encryption
{
    public sealed class MasterPasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 20;

        public string HashedPassword = "";
        public string SaltValue = "";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_password"></param>
        /// <returns>String</returns>
        public void HashPassword(string _password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            var pbkdf2 = new Rfc2898DeriveBytes(_password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            this.HashedPassword = Convert.ToBase64String(hashBytes);
            this.SaltValue = Convert.ToBase64String(salt);
        }

        public bool VerifyPassword(string _password)
        {
            //var iterations = Int32.Parse(splittedHashString[0]);
            //var base64Hash = splittedHashString[1];

            byte[] hashBytes = Convert.FromBase64String(HashedPassword);

            byte[] salt = Convert.FromBase64String(SaltValue);
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            var pbkdf2 = new Rfc2898DeriveBytes(_password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
