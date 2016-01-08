using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Web;

namespace ADIdentityServer.Models
{
    public class ChangePasswordModel
    {
        public string Account { get; set; }
        [Required]
        [Display(Name = "Mật khẩu cũ")]
        public string OldPassword { get; set; }
        [Required]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }
        [Required]
        [Display(Name = "Nhập lại mật khẩu")]
        public string ConfirmNewPassword { get; set; }
    }
}