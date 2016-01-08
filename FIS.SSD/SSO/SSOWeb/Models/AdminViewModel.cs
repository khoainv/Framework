using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SSD.SSO.Identity;

namespace SSOWeb.Models
{
    public class RoleViewModel
    {
        public RoleViewModel()
        {
            this.PerrmissionList = new List<SelectListItem>();
            this.GroupPerrmissionList = new Dictionary<string, IEnumerable<SelectListItem>>();
        }
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "RoleName")]
        public string Name { get; set; }
        public string Description { get; set; }

        public IEnumerable<SelectListItem> PerrmissionList { get; set; }
        public IDictionary<string,IEnumerable<SelectListItem>> GroupPerrmissionList { get; set; }
    }

    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            this.RolesList = new List<SelectListItem>();
        }

        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        // Add the Address Info:
        public string Address { get; set; }
        public string City { get; set; }
        public string Contry { get; set; }
        public bool IsSystemAccount { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
    public class ListPermissionViewModel
    {
        public ListPermissionViewModel()
        {
            this.PermissionList = new List<ApplicationPermission>();
        }
        public ICollection<ApplicationPermission> PermissionList { get; set; }
    }
    public class PermissionViewModel
    {
        public PermissionViewModel()
        {
            this.RolesList = new List<SelectListItem>();
        }
        [Required(AllowEmptyStrings = false)]
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string ControllerKey { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string AcctionKey { get; set; }
        public string Description { get; set; }
        public ICollection<SelectListItem> RolesList { get; set; }
    }
}