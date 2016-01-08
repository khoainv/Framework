using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using SSD.Framework;
using SSD.Framework.Cryptography;
using SSD.Framework.Security;

namespace SSD.Web.Security
{
    public class UGToken:BaseToken
    {
        public UGToken()
        { }
        public UGToken(UserAuthen user):base(user)
        {
            UID = user.UserName;
            PWD = user.Password;
            Hash = user.PasswordHash;
            Exp = user.ExpireTimeSpanHours;
            CRT = DateTime.Now.Ticks;
        }
        public UGToken(UserAuthen user, string profileJson):base(user,profileJson)
        {
        }
        
        public string Encrypt()
        {
            return RSAEngine.Password.EncryptPassword(this.ToString());
        }

        public static UGToken Decrypt(string encryptedToken)
        {
            string decrypted = RSAEngine.Password.DecryptPassword(encryptedToken);

            return JsonConvert.DeserializeObject<UGToken>(decrypted);
        }
    }
}
