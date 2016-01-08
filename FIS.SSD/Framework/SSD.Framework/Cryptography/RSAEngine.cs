#region

using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

#endregion

namespace SSD.Framework.Cryptography
{
    public class RSAEngine
    {
        #region Old
        /*
        public static RSACryptoServiceProvider rsa;
        private static HashAlgorithm x_hash = HashAlgorithm.Create("SHA1");
        public const string publicOnlyKeyXMLDefault = "<RSAKeyValue><Modulus>nsarD/Rn+nkKM0modvsK1ff/e3TCPb/kB9j/VXzXpind39rSBxlxEBsscR5a9nMKGN0uo0yd7wAYySoSvYkc9GzCN8mSafCdl1wTo/O95xbqJNP3HABxqHMHNv6zDCAXXJmdW/zQBKAYqpDq/EIKwctKXXvOZLRRRy4M/TIxqYc=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        public const string publicPrivateKeyXMLDefault = "<RSAKeyValue><Modulus>nsarD/Rn+nkKM0modvsK1ff/e3TCPb/kB9j/VXzXpind39rSBxlxEBsscR5a9nMKGN0uo0yd7wAYySoSvYkc9GzCN8mSafCdl1wTo/O95xbqJNP3HABxqHMHNv6zDCAXXJmdW/zQBKAYqpDq/EIKwctKXXvOZLRRRy4M/TIxqYc=</Modulus><Exponent>AQAB</Exponent><P>2BxbTAUB51Eq1N22gl2k/0MGvlyJ3FWSZVgmY8n6FyAK7/c6zRQyh+KAfONqSyOnqJiO2TjG3YSa498r0uR/2w==</P><Q>vBUjK4wS3/ctN3SQfv6SSc9P15Fqgp9NVDN2q+agQmlYdMXwrUzOtNGRRQ+OMzXeL7ETCMSppNxkmrmWHu+yxQ==</Q><DP>o1fzjabvRGauKAyYiTq8no+LxlBthxNKvrz870nXdKkseynz0NQmSVzi3wKI8dg2PhFpTzhB32b+J6QkHJfHJw==</DP><DQ>g8OnLQXn30HSSqx94opEQDNdsx7r6HDkAt4/ADUFByG4V66oazCJC8JZrHE2ZQgTDYRXWmg0lQvV61OjKS8yJQ==</DQ><InverseQ>Gp9WyvdtM1TwFweVaFe7VPz5DtpdiRMoWASUtoXP6z+4mgVUzOUaXS49QCY4D8K9UgMip3z+Yp66YVHmBiuEMQ==</InverseQ><D>SNEEEsUcCpsIOo1FXYu0ZHzgBlZ93qPqiE7Uivg+Tk5VGxzXxbm9SI2tzBMH1I7dypllki7JH5sDwv4wqgv/4r+/X75kLhJOrkJ9g4sFkgLtPgDlYe+Fsuid7vLWRCezZN8DEjw/fWZP9GS3CcLtvXislVztn5BnifjFbYcPVkE=</D></RSAKeyValue>";
        
        public static void AssignParameter()
        {
            const int PROVIDER_RSA_FULL = 1;
            const string CONTAINER_NAME = "SpiderContainer";
            CspParameters cspParams;
            cspParams = new CspParameters(PROVIDER_RSA_FULL);
            cspParams.KeyContainerName = CONTAINER_NAME;
            cspParams.Flags = CspProviderFlags.UseMachineKeyStore;
            cspParams.ProviderName = "Microsoft Strong Cryptographic Provider";
            rsa = new RSACryptoServiceProvider(cspParams);
        }
        #region Get from File

        public static X509Certificate2 LoadFromFile(string filename, string password)
        {
            try
            {

                X509Certificate2 x509 = new X509Certificate2(Utility.GetFileBytes(filename), password, X509KeyStorageFlags.UserKeySet | X509KeyStorageFlags.Exportable);// | X509KeyStorageFlags.UserProtected | X509KeyStorageFlags.PersistKeySet);
                return x509;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static X509Certificate2 LoadFromFile(string filename)
        {
            try
            {
                X509Certificate2 x509 = new X509Certificate2(filename);

                return x509;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetPrivateXMLFromFilePFX(string filename, string password)
        {
            try
            {
                X509Certificate2 x509 = LoadFromFile(filename, password);

                return x509.HasPrivateKey ? x509.PrivateKey.ToXmlString(true) : string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string GetPublicKeyXMLFromFile(string filename, string password)
        {
            try
            {
                X509Certificate2 x509 = LoadFromFile(filename, password);
                return x509.PublicKey.Key.ToXmlString(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string GetPublicKeyXMLFromFile(string filename)
        {
            try
            {
                X509Certificate2 x509 = LoadFromFile(filename);
                return x509.PublicKey.Key.ToXmlString(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Sign Data
        public static string AddZero(string temp)
        {
            int i = temp.Length % 4;
            if (i > 0)
            {
                for (int j = 0; j < 4 - i; j++)
                    temp += "0";
            }
            return temp;
        }
        public static string SignDataFromFile(string plaintext, string fileName, string password)
        {
            return SignData(plaintext, GetPrivateXMLFromFilePFX(fileName, password));
        }
        public static string SignData(string plaintext)
        {
            return SignData(plaintext, publicPrivateKeyXMLDefault);
        }
        private static string ProcessPlaintext(string plaintext)
        {
            plaintext = plaintext.Replace(".", "");
            return AddZero(plaintext);
        }
        private static byte[] FromBase64String(string plaintext)
        {
            plaintext = plaintext == null ? string.Empty : plaintext;

            string temp = ProcessPlaintext(plaintext);

            return Convert.FromBase64String(temp);
        }
        public static string SignData(string plaintext, string publicPrivateKeyXML)
        {
            try
            {
                AssignParameter();

                rsa.FromXmlString(publicPrivateKeyXML);

                byte[] x_plaintext = FromBase64String(plaintext);
                // create an instance of the SHA-1 hashing algorithm

                byte[] x_rsa_signature = rsa.SignData(x_plaintext, x_hash);

                string signValue = Convert.ToBase64String(x_rsa_signature);
                return signValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        #region VerifySignature
        public static bool VerifySignatureFromFile(string plaintext, string signValue, string filename)
        {
            return VerifySignature(plaintext, signValue, GetPublicKeyXMLFromFile(filename));
        }
        public static bool VerifySignatureFromFile(string plaintext, string signValue, string filename, string password)
        {
            return VerifySignature(plaintext, signValue, GetPublicKeyXMLFromFile(filename, password));
        }
        public static bool VerifySignature(string plaintext, string signValue)
        {
            return VerifySignature(plaintext, signValue, publicOnlyKeyXMLDefault);
        }

        public static bool VerifySignature(string plaintext, string signValue, string publicOnlyKeyXML)
        {
            try
            {
                AssignParameter();
                rsa.FromXmlString(publicOnlyKeyXML);

                byte[] x_plaintext = FromBase64String(plaintext);//Encoding...UTF8.GetBytes(plaintext);

                // Convert base64-encoded hash value into a byte array.
                byte[] x_signature;
                try
                {
                    x_signature = Convert.FromBase64String(signValue);
                }
                catch
                {
                    return false;
                }

                // create an instance of the SHA-1 hashing algorithm
                bool x_rsa_sig_valid = rsa.VerifyData(x_plaintext, x_hash, x_signature);

                return x_rsa_sig_valid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Decrypt Data
        public static string DecryptData(string data2Decrypt)
        {
            return DecryptData(data2Decrypt, publicPrivateKeyXMLDefault);
        }
        public static string DecryptData(string[] data2Decrypt)
        {
            string enc = string.Empty;
            for (int i = 0; i < data2Decrypt.Length; i++)
            {
                if (i < (data2Decrypt.Length - 1))
                    enc = enc + DecryptData(data2Decrypt[i], publicPrivateKeyXMLDefault) + ";";
                else enc = enc + DecryptData(data2Decrypt[i], publicPrivateKeyXMLDefault);

            }
            return enc;
        }
        public static string DecryptDataFromFile(string data2Decrypt, string fileName, string password)
        {
            return DecryptData(data2Decrypt, GetPrivateXMLFromFilePFX(fileName, password));
        }
        public static string DecryptData(string data2Decrypt, string publicPrivateKeyXML)
        {
            AssignParameter();
            byte[] getpassword = Convert.FromBase64String(data2Decrypt);
            rsa.FromXmlString(publicPrivateKeyXML);
            //read ciphertext, decrypt it to plaintext	
            byte[] plain = rsa.Decrypt(getpassword, false);
            return System.Text.Encoding.UTF8.GetString(plain);
        }
        #endregion
        #region Encrypt Data
        public static string EncryptDataFromFile(string data2Encrypt, string fileName, string password)
        {
            return EncryptData(data2Encrypt, GetPublicKeyXMLFromFile(fileName, password));
        }
        public static string EncryptDataFromFile(string data2Encrypt, string fileName)
        {
            return EncryptData(data2Encrypt, GetPublicKeyXMLFromFile(fileName));
        }
        public static string EncryptData(string data2Encrypt, string publicOnlyKeyXML)
        {
            try
            {
                AssignParameter();
                rsa.FromXmlString(publicOnlyKeyXML);
                //read plaintext, encrypt it to ciphertext
                byte[] plainbytes = System.Text.Encoding.UTF8.GetBytes(data2Encrypt);
                byte[] cipherbytes = rsa.Encrypt(plainbytes, false);
                return Convert.ToBase64String(cipherbytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string EncryptData(string data2Encrypt)
        {
            return EncryptData(data2Encrypt, publicOnlyKeyXMLDefault);
        }
        public static string EncryptData(string[] data2Encrypt)
        {
            string enc = string.Empty;
            for (int i = 0; i < data2Encrypt.Length; i++)
            {
                if (i < (data2Encrypt.Length - 1))
                    enc = enc + EncryptData(data2Encrypt[i], publicOnlyKeyXMLDefault) + ";";
                else enc = enc + EncryptData(data2Encrypt[i], publicOnlyKeyXMLDefault);
            }
            return enc;
        }
        #endregion
        */
        #endregion

        #region Load Key
        public static string GetPublicKeyToString(string filename)
        {
            string publicKey = GetFileString(filename);
            publicKey = publicKey.Replace("-----BEGIN CERTIFICATE-----", "");
            publicKey = publicKey.Replace("-----END CERTIFICATE-----", "");
            return publicKey;
        }
        public static string GetFileString(string filename)
        {
            try
            {
                StreamReader re = File.OpenText(filename);
                string input = null;
                input = re.ReadToEnd();
                re.Close();
                return input;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static byte[] GetFileBytes(String filename)
        {
            if (!File.Exists(filename))
                return null;
            Stream stream = new FileStream(filename, FileMode.Open);
            int datalen = (int)stream.Length;
            byte[] filebytes = new byte[datalen];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(filebytes, 0, datalen);
            stream.Close();
            return filebytes;
        }
        public static string GetPrivateXMLFromFilePFX(string filename, string password)
        {
            try
            {
                X509Certificate2 x509 = LoadFromFile(filename, password);

                return x509.HasPrivateKey ? x509.PrivateKey.ToXmlString(true) : string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetSerialNumber(string publicKey)
        {
            try
            {
                byte[] bInput;
                bInput = Convert.FromBase64String(publicKey);
                X509Certificate2 x509 = new X509Certificate2();
                x509.Import(bInput);
                return x509.SerialNumber;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static X509Certificate2 LoadFromFile(string filename, string password)
        {
            try
            {
                X509Certificate2 x509 = new X509Certificate2(GetFileBytes(filename), password, X509KeyStorageFlags.UserKeySet | X509KeyStorageFlags.Exportable);// | X509KeyStorageFlags.UserProtected | X509KeyStorageFlags.PersistKeySet);
                return x509;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 

        #region Signature, Verify

        public static string SignData(string plaintext, string filename, string password)
        {
            try
            {
                X509Certificate2 x509 = LoadFromFile(filename, password);
                RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)(x509.PrivateKey);

                //RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                //rsa.FromXmlString(privateKey);
                int keySize = rsa.KeySize;

                //byte[] x_plaintext = Convert.FromBase64String(plaintext);
                // create an instance of the SHA-1 hashing algorithm

                byte[] data = UnicodeEncoding.UTF8.GetBytes(plaintext);
                SHA1 sha = new SHA1CryptoServiceProvider();
                byte[] hash = sha.ComputeHash(data);

                //SHA1 sha = HashAlgorithm.Create("SHA1");
                //rsa.SignData(x_plaintext,sha);

                byte[] x_rsa_signature = rsa.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));

                string signValue = Convert.ToBase64String(x_rsa_signature);
                return signValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool VerifySign(string plaintext, string publicKey, string signature)
        {
            byte[] bInput;
            bInput = Convert.FromBase64String(publicKey);
            X509Certificate2 x509 = new X509Certificate2();
            x509.Import(bInput);

            # region 1. Check Chữ ký còn hạn hay không?
            // 1. Check Chữ ký còn hạn hay không?
            DateTime expirationDate = DateTime.Parse(x509.GetExpirationDateString());
            if (expirationDate.CompareTo(DateTime.Now) <= 0)
                throw new Exception("Chữ ký không còn hạn sử dụng");
            # endregion

            # region 2. Check chữ ký có bị thu hồi hay không?
            // 2. Check chữ ký có bị thu hồi hay không?
            //check ở tầng trên
            // Đoạn này trở lên con web service của em: số serial number lấy bằng số: x509.SerialNumber
            # endregion

            # region 3. Check Root
            // 3. Check Root
            //error if (!x509.Verify()) throw new VerifySignatureHQException("Lỗi check root publickey");

            bool valid = false;
            X509Certificate2 crt2 = FindIssuer(x509);
            if (crt2 == null) valid = FindRoot(x509);
            else valid = FindRoot(crt2);

            //if (!valid) throw new VerifySignatureHQException("Khóa là sai root");
            # endregion

            # region 4. Xác thực chữ ký có đúng không? (Check Signature)
            // 4. Xác thực chữ ký có đúng không? (Check Signature)
            RSACryptoServiceProvider CSP = (RSACryptoServiceProvider)(x509.PublicKey.Key);
            byte[] data = UnicodeEncoding.UTF8.GetBytes(plaintext);
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] hash = sha.ComputeHash(data);
            byte[] signatureByte = System.Convert.FromBase64String(signature);
            //VerifyHash
            if (!CSP.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), signatureByte)) throw new Exception("Xác minh chữ ký không thành công");
            # endregion
            return true;
        }

        private static bool FindRoot(X509Certificate2 certificate)
        {
            bool find = false;
            X509Store store = new X509Store(StoreName.Root);
            store.Open(OpenFlags.ReadOnly);
            foreach (X509Certificate2 cert in store.Certificates)
            {
                if (cert.Issuer == certificate.Issuer)
                {
                    find = true;
                }
            }
            return find;
        }

        private static X509Certificate2 FindIssuer(X509Certificate2 certificate)
        {
            X509Certificate2 IntermediateCert = null;
            try
            {
                X509Store store = new X509Store(StoreName.CertificateAuthority);
                store.Open(OpenFlags.ReadOnly);
                foreach (X509Certificate2 cert in store.Certificates)
                {
                    if (cert.Subject == certificate.Issuer)
                    {
                        IntermediateCert = FindIssuer(cert);
                        if (IntermediateCert == null)
                            IntermediateCert = cert;
                    }
                }
            }
            catch
            {
                return null;
            }
            return IntermediateCert;

        }
        #endregion

        #region Decrypt and Encrypt
        public static string DecryptData(string cipherbytes, string filename, string password)
        {
            try
            {
                X509Certificate2 x509 = LoadFromFile(filename, password);
                RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)(x509.PrivateKey);
                int keySize = x509.PrivateKey.KeySize;

                //RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                //rsa.FromXmlString(privateKey);
                //int keySize = rsa.KeySize;
                

                int base64BlockSize = ((keySize / 8) % 3 != 0) ?
                  (((keySize / 8) / 3) * 4) + 4 : ((keySize / 8) / 3) * 4;
                int iterations = cipherbytes.Length / base64BlockSize;
                ArrayList arrayList = new ArrayList();
                for (int i = 0; i < iterations; i++)
                {
                    byte[] encryptedBytes = Convert.FromBase64String(
                         cipherbytes.Substring(base64BlockSize * i, base64BlockSize));
                    // Be aware the RSACryptoServiceProvider reverses the order of 
                    // encrypted bytes after encryption and before decryption.
                    // If you do not require compatibility with Microsoft Cryptographic 
                    // API (CAPI) and/or other vendors.
                    // Comment out the next line and the corresponding one in the 
                    // EncryptString function.
                    Array.Reverse(encryptedBytes);
                    arrayList.AddRange(rsa.Decrypt(encryptedBytes, true));
                }
                return System.Text.Encoding.UTF8.GetString(arrayList.ToArray(Type.GetType("System.Byte")) as byte[]);//Encoding.UTF32.GetString(arrayList.ToArray(Type.GetType("System.Byte")) as byte[]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string EncryptData(string plaintext, string publicKey)
        {
            byte[] bInput = Convert.FromBase64String(publicKey);
            X509Certificate2 x509 = new X509Certificate2();
            x509.Import(bInput);
            RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)(x509.PublicKey.Key);
            //read plaintext, encrypt it to ciphertext
            byte[] plainbytes = System.Text.Encoding.UTF8.GetBytes(plaintext);

            // The hash function in use by the .NET RSACryptoServiceProvider here 
            // is SHA1
            // int maxLength = ( keySize ) - 2 - 
            //              ( 2 * SHA1.Create().ComputeHash( rawBytes ).Length );
            int maxLength = rsa.KeySize / 8 - 42;
            int dataLength = plainbytes.Length;
            int iterations = dataLength / maxLength;
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i <= iterations; i++)
            {
                byte[] tempBytes = new byte[
                        (dataLength - maxLength * i > maxLength) ? maxLength :
                                                      dataLength - maxLength * i];
                Buffer.BlockCopy(plainbytes, maxLength * i, tempBytes, 0,
                                  tempBytes.Length);

                byte[] cipherbytes = rsa.Encrypt(tempBytes, true);
                // Be aware the RSACryptoServiceProvider reverses the order of 
                // encrypted bytes. It does this after encryption and before 
                // decryption. If you do not require compatibility with Microsoft 
                // Cryptographic API (CAPI) and/or other vendors. Comment out the 
                // next line and the corresponding one in the DecryptString function.
                Array.Reverse(cipherbytes);
                // Why convert to base 64?
                // Because it is the largest power-of-two base printable using only 
                // ASCII characters
                stringBuilder.Append(Convert.ToBase64String(cipherbytes));

            }
            return stringBuilder.ToString();
        }
        public static string EncryptData(string[] aryPlaintext, string publicKey)
        {
            string enc = string.Empty;
            for (int i = 0; i < aryPlaintext.Length; i++)
            {
                if (i < (aryPlaintext.Length - 1))
                    enc = enc + EncryptData(aryPlaintext[i], publicKey) + ";";
                else enc = enc + EncryptData(aryPlaintext[i], publicKey);
            }
            return enc;
        }
        #endregion

        public class Password
        {
            public const string publicOnlyKeyXMLDefault = "<RSAKeyValue><Modulus>nsarD/Rn+nkKM0modvsK1ff/e3TCPb/kB9j/VXzXpind39rSBxlxEBsscR5a9nMKGN0uo0yd7wAYySoSvYkc9GzCN8mSafCdl1wTo/O95xbqJNP3HABxqHMHNv6zDCAXXJmdW/zQBKAYqpDq/EIKwctKXXvOZLRRRy4M/TIxqYc=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            public const string publicPrivateKeyXMLDefault = "<RSAKeyValue><Modulus>nsarD/Rn+nkKM0modvsK1ff/e3TCPb/kB9j/VXzXpind39rSBxlxEBsscR5a9nMKGN0uo0yd7wAYySoSvYkc9GzCN8mSafCdl1wTo/O95xbqJNP3HABxqHMHNv6zDCAXXJmdW/zQBKAYqpDq/EIKwctKXXvOZLRRRy4M/TIxqYc=</Modulus><Exponent>AQAB</Exponent><P>2BxbTAUB51Eq1N22gl2k/0MGvlyJ3FWSZVgmY8n6FyAK7/c6zRQyh+KAfONqSyOnqJiO2TjG3YSa498r0uR/2w==</P><Q>vBUjK4wS3/ctN3SQfv6SSc9P15Fqgp9NVDN2q+agQmlYdMXwrUzOtNGRRQ+OMzXeL7ETCMSppNxkmrmWHu+yxQ==</Q><DP>o1fzjabvRGauKAyYiTq8no+LxlBthxNKvrz870nXdKkseynz0NQmSVzi3wKI8dg2PhFpTzhB32b+J6QkHJfHJw==</DP><DQ>g8OnLQXn30HSSqx94opEQDNdsx7r6HDkAt4/ADUFByG4V66oazCJC8JZrHE2ZQgTDYRXWmg0lQvV61OjKS8yJQ==</DQ><InverseQ>Gp9WyvdtM1TwFweVaFe7VPz5DtpdiRMoWASUtoXP6z+4mgVUzOUaXS49QCY4D8K9UgMip3z+Yp66YVHmBiuEMQ==</InverseQ><D>SNEEEsUcCpsIOo1FXYu0ZHzgBlZ93qPqiE7Uivg+Tk5VGxzXxbm9SI2tzBMH1I7dypllki7JH5sDwv4wqgv/4r+/X75kLhJOrkJ9g4sFkgLtPgDlYe+Fsuid7vLWRCezZN8DEjw/fWZP9GS3CcLtvXislVztn5BnifjFbYcPVkE=</D></RSAKeyValue>";
            public static string EncryptPassword(string plaintext)
            {
                if (string.IsNullOrWhiteSpace(plaintext))
                    return plaintext;
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(publicOnlyKeyXMLDefault);
                //read plaintext, encrypt it to ciphertext
                byte[] plainbytes = System.Text.Encoding.UTF8.GetBytes(plaintext);

                // The hash function in use by the .NET RSACryptoServiceProvider here 
                // is SHA1
                // int maxLength = ( keySize ) - 2 - 
                //              ( 2 * SHA1.Create().ComputeHash( rawBytes ).Length );
                int maxLength = rsa.KeySize / 8 - 42;
                int dataLength = plainbytes.Length;
                int iterations = dataLength / maxLength;
                StringBuilder stringBuilder = new StringBuilder();

                for (int i = 0; i <= iterations; i++)
                {
                    byte[] tempBytes = new byte[
                            (dataLength - maxLength * i > maxLength) ? maxLength :
                                                          dataLength - maxLength * i];
                    Buffer.BlockCopy(plainbytes, maxLength * i, tempBytes, 0,
                                      tempBytes.Length);

                    byte[] cipherbytes = rsa.Encrypt(tempBytes, true);
                    // Be aware the RSACryptoServiceProvider reverses the order of 
                    // encrypted bytes. It does this after encryption and before 
                    // decryption. If you do not require compatibility with Microsoft 
                    // Cryptographic API (CAPI) and/or other vendors. Comment out the 
                    // next line and the corresponding one in the DecryptString function.
                    Array.Reverse(cipherbytes);
                    // Why convert to base 64?
                    // Because it is the largest power-of-two base printable using only 
                    // ASCII characters
                    stringBuilder.Append(Convert.ToBase64String(cipherbytes));

                }
                return stringBuilder.ToString();
            }
            public static string DecryptPassword(string cipherbytes)
            {
                if (string.IsNullOrWhiteSpace(cipherbytes))
                    return cipherbytes;
                try
                {
                    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                    rsa.FromXmlString(publicPrivateKeyXMLDefault);
                    int keySize = rsa.KeySize;


                    int base64BlockSize = ((keySize / 8) % 3 != 0) ?
                      (((keySize / 8) / 3) * 4) + 4 : ((keySize / 8) / 3) * 4;
                    int iterations = cipherbytes.Length / base64BlockSize;
                    ArrayList arrayList = new ArrayList();
                    for (int i = 0; i < iterations; i++)
                    {
                        byte[] encryptedBytes = Convert.FromBase64String(
                             cipherbytes.Substring(base64BlockSize * i, base64BlockSize));
                        // Be aware the RSACryptoServiceProvider reverses the order of 
                        // encrypted bytes after encryption and before decryption.
                        // If you do not require compatibility with Microsoft Cryptographic 
                        // API (CAPI) and/or other vendors.
                        // Comment out the next line and the corresponding one in the 
                        // EncryptString function.
                        Array.Reverse(encryptedBytes);
                        arrayList.AddRange(rsa.Decrypt(encryptedBytes, true));
                    }
                    return System.Text.Encoding.UTF8.GetString(arrayList.ToArray(Type.GetType("System.Byte")) as byte[]);//Encoding.UTF32.GetString(arrayList.ToArray(Type.GetType("System.Byte")) as byte[]);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }

}
