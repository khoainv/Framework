using System.ComponentModel.DataAnnotations;

namespace UG.Web.Security
{
    public class UserAuthen
    {
        public UserAuthen()
        {
        }
        public UserAuthen(string userName, string password, string passwordHash, int expireTimeSpanHours)
        {
            UserName = userName;
            Password = password;
            PasswordHash = passwordHash;
            ExpireTimeSpanHours = expireTimeSpanHours;
        }
        public UserAuthen(UGToken token)
        {
            UserName = token.UID;
            Password = token.PWD;
            PasswordHash = token.Hash;
            ExpireTimeSpanHours = token.Exp;
        }
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [StringLength(256)]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(256)]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Expire TimeSpan Hours")]
        public int ExpireTimeSpanHours { get; set; }

        //First login
        [Required]
        [Display(Name = "Mac Address")]
        [StringLength(50)]
        public string MacAddress { get; set; }
        [Display(Name = "Password Hashed")]
        public string PasswordHash { get; set; }
        
    }
}
