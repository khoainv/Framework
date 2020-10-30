#region

using PCLCrypto;
using System;

#endregion

namespace SSD.Framework.Cryptography
{
    public class AESEngine
    {
        public static string CreateSalt(uint lengthInBytes=16)
        {
            var salt = WinRTCrypto.CryptographicBuffer.GenerateRandom(lengthInBytes);
            return Convert.ToBase64String(salt);
        }
        private static byte[] CreateDerivedKey(string password, string salt, int keyLengthInBytes = 32, int iterations = 1000)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] key = NetFxCrypto.DeriveBytes.GetBytes(password, saltBytes, iterations, keyLengthInBytes);
            return key;
        }
        public static string Encryption(string plainText,string password, string salt)
        {
            byte[] keyMaterial = CreateDerivedKey(password, salt);
            byte[] plainbytes = System.Text.Encoding.UTF8.GetBytes(plainText);

            var provider = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
            var key = provider.CreateSymmetricKey(keyMaterial);
            byte[] iv = null; // this is optional, but must be the same for both encrypting and decrypting
            byte[] cipherbytes = WinRTCrypto.CryptographicEngine.Encrypt(key, plainbytes, iv);

            return Convert.ToBase64String(cipherbytes);
        }
        public static string Decryption(string cipherText,string password, string salt)
        {
            byte[] keyMaterial = CreateDerivedKey(password, salt);
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            var provider = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
            var key = provider.CreateSymmetricKey(keyMaterial);
            byte[] plainBytes = WinRTCrypto.CryptographicEngine.Decrypt(key, cipherBytes);

            string plainText = System.Text.Encoding.UTF8.GetString(plainBytes, 0, plainBytes.Length);
            return plainText;
        }
        public static string Encryption(string plainText, string salt)
        {
            return Encryption(plainText, "abcde12345-", salt);
        }
        public static string Decryption(string cipherText, string salt)
        {
            return Decryption(cipherText, "abcde12345-", salt);
        }
    }
}
