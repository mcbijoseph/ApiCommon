using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    /// <summary>
    /// PASSWORD SECURITY
    /// </summary>
    public class PasswordSecurity
    {
        private const int SaltSize = 16;

        private const int HashSize = 20;

        private const int Iterations = 10000;

        private const string defaultPassword = "Password1";


        public static void Main(string[]args)
        {
            Console.Write("Hello:");
            string s = Hash("joseph");
            Console.WriteLine(s);
            Console.WriteLine("Unhashed:{0} - {1}", Unhash(s), VerifyPassword("joseph", s));
            Console.ReadKey();
        }

        public static string Hash(string password)
        {
            byte[] salt;

            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            var hashedPassword = Convert.ToBase64String(hashBytes);

            return hashedPassword;

        }

        public static bool VerifyPassword(string userPassword, string savedPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(savedPassword);

            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            var pbkdf2 = new Rfc2898DeriveBytes(userPassword, salt, Iterations);

            byte[] hash = pbkdf2.GetBytes(HashSize);

            for (int i = 0; i < HashSize; i++)
                if (hashBytes[i + SaltSize] != hash[i])
                    return false;

            return true;

        }
        public static string Unhash(string savedPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(savedPassword);
            return Convert.ToBase64String(hashBytes);
        }

        public static string GetDefaultPassword()
        {
            return Hash(defaultPassword);
        }
    }
}
