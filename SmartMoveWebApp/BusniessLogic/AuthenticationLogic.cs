using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace SmartMoveWebApp.BusniessLogic
{
    public class AuthenticationLogic
    {
        private static byte[] Get_SALT()
        {
            return Get_SALT(64);
        }

        public static byte[] Get_SALT(int maximumSaltLength)
        {
            var salt = new byte[maximumSaltLength];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }
            return salt;
        }

        public static string Get_HASH_SHA512(string password, string username, byte[] salt)
        {
            try
            {
                //Plain Text in Byte  
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(password + username);

                //Plain Text + SALT Key in Byte  
                byte[] plainTextWithSaltBytes = new byte[plainTextBytes.Length + salt.Length];

                for (int i = 0; i < plainTextBytes.Length; i++)
                {
                    plainTextWithSaltBytes[i] = plainTextBytes[i];
                }

                for (int i = 0; i < salt.Length; i++)
                {
                    plainTextWithSaltBytes[plainTextBytes.Length + i] = salt[i];
                }

                HashAlgorithm hash = new SHA512Managed();
                byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);
                byte[] hashWithSaltBytes = new byte[hashBytes.Length + salt.Length];

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    hashWithSaltBytes[i] = hashBytes[i];
                }

                for (int i = 0; i < salt.Length; i++)
                {
                    hashWithSaltBytes[hashBytes.Length + i] = salt[i];
                }

                return Convert.ToBase64String(hashWithSaltBytes);
            }
            catch
            {
                return string.Empty;
            }
        }
        public static bool CompareHashValue(string password, string username, string OldHASHValue, byte[] SALT)
        {
            try
            {
                string expectedHashString = AuthenticationLogic.Get_HASH_SHA512(password, username, SALT);

                return (OldHASHValue == expectedHashString);
            }
            catch
            {
                return false;
            }
        }
    }
}