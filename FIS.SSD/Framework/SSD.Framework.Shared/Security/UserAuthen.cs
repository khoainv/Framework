namespace SSD.Framework.Security
{
    public class UserAuthen: BaseInput
    {
        public UserAuthen()
        {
        }
        public UserAuthen(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
        public UserAuthen(string userName, string password, string macAddress, int expireTimeSpanHours)
        {
            UserName = userName;
            Password = password;
            MacAddress = macAddress;
            ExpireTimeSpanHours = expireTimeSpanHours;
        }
        public UserAuthen(string userName, string password, string macAddress, int expireTimeSpanHours, string passwordHash)
        {
            UserName = userName;
            Password = password;
            PasswordHash = passwordHash;
            MacAddress = macAddress;
            ExpireTimeSpanHours = expireTimeSpanHours;
        }
        public UserAuthen(BaseToken token)
        {
            UserName = token.UID;
            Password = token.PWD;
            PasswordHash = token.Hash;
            ExpireTimeSpanHours = token.Exp;
        }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int ExpireTimeSpanHours { get; set; }
        //First login
        public string MacAddress { get; set; }
        public string PasswordHash { get; set; }
        
    }
}
