using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ISMNewsPortal.Models
{
    public static class Security
    {
        public const int PasswordUserLength = 128;

        public static string SHA512(string input, out string salt)
        {
            salt = RandomString(PasswordUserLength);
            return SHA512(input, salt);
        }

        public static string SHA512(string input, string salt)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var saltBytes = Encoding.UTF8.GetBytes(salt);
            bytes = bytes.Concat(saltBytes).ToArray();
            Array.Resize(ref bytes, PasswordUserLength); // <<===
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);
                return Encoding.UTF8.GetString(hashedInputBytes);
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