using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Extensions
{
    public static class ByteArrayExtensions
    {
        public static byte[] GenerateHash(this byte[] input)
        {
            SHA1 sha1 = SHA1.Create();
            return sha1.ComputeHash(input);
        }

        public static string GenerateHashBase64String(this byte[] input)
        {
            if (input == null) return string.Empty;

            if (input.Length == 0) return string.Empty;

            return Convert.ToBase64String(GenerateHash(input));
        }
    }
}
