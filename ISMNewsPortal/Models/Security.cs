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
        [ThreadStatic]
        private static Random random;
        public static Random Random
        {
            get
            {
                return random ?? (random = new Random((int)DateTime.Now.Ticks));
            }
        }
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
            Array.Resize(ref bytes, 128);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);
                //var hashedInputStringBuilder = new StringBuilder(128);
                //foreach (var b in hashedInputBytes)
                //    hashedInputStringBuilder.Append(b.ToString("X2"));
                return Encoding.UTF8.GetString(hashedInputBytes);
            }
        }
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}