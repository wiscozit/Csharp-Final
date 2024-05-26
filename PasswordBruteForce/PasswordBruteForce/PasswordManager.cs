using System;
using System.Security.Cryptography;
using System.Text;

namespace PasswordBruteForce
{
    public class PasswordManager
    {

        public static string EncryptPassword(string password, string Salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = password + Salt;
                var saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
                var hashBytes = sha256.ComputeHash(saltedPasswordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        public static bool VerifyPassword(string password, string salt, string encryptedPassword)
        {
            var encryptedAttempt = EncryptPassword(password,salt);
            return encryptedAttempt == encryptedPassword;
        }
    }
}