using Newtonsoft.Json;
using System;

namespace SSD.Framework.Security
{
    public class BaseToken
    {
        public BaseToken()
        { }
        public BaseToken(UserAuthen user)
        {
            UID = user.UserName;
            PWD = user.Password;
            Hash = user.PasswordHash;
            Exp = user.ExpireTimeSpanHours;
            CRT = DateTime.Now.Ticks;
        }
        public BaseToken(UserAuthen user, string profileJson)
        {
            UID = user.UserName;
            PWD = user.Password;
            Hash = user.PasswordHash;
            Exp = user.ExpireTimeSpanHours;
            PRF = profileJson;
            CRT = DateTime.Now.Ticks;
        }
        //UserId
        public string UID { get; set; }
        //Password
        public string PWD { get; set; }
        //PasswordHash
        public string Hash { get; set; }
        //ExpireTimeSpanHours
        public int Exp { get; set; }
        //CreatedDate
        public long CRT { get; set; }
        //ProfileJson
        public string PRF
        {
            get;
            set;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
