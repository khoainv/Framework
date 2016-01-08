using System;
using Replicon.Cryptography.SCrypt;

namespace  SSD.Framework.Hashing.SCRYPT
{
    public static class SCryptWapper
    {
        public static string HashPassword(string password)
        {
            return SCrypt.HashPassword(password);
        }
        public static string HashPassword(string password, ulong iterations)
        {
            iterations = (ulong)Math.Pow(2, iterations);
            string saft = SCrypt.GenerateSalt(SCrypt.DefaultSaltLengthBytes, iterations, SCrypt.Default_r, SCrypt.Default_p, SCrypt.DefaultHashLengthBytes);
            return SCrypt.HashPassword(password, saft);
        }

        public static bool VerifyPassword(string plaintext, string hashed)
        {
            return SCrypt.Verify(plaintext, hashed);
        }
    }
}
