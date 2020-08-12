using System;
using System.Linq;
using System.Text;

namespace ISMNewsPortal.Models
{
    public static class Security
    {
        public const int PasswordUserLength = 64;

        public static string SHA512(string input, out string salt)
        {
            salt = RandomString(PasswordUserLength);
            return SHA512(input, salt);
        }

        public static string SHA512(string input, string salt)
        {
            var saltedPassword = input + salt;
            var bytes = Encoding.UTF8.GetBytes(saltedPassword);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);
                var hashedInputStringBuilder = new StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
            }
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}