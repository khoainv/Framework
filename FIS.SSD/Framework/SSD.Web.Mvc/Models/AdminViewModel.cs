using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SSD.Web.Models;

namespace SSD.Web.Mvc.Models
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
            this.GroupsList = new List<SelectListItem>();
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

        public IEnumerable<SelectListItem> GroupsList { get; set; }
        public IEnumerable<Permission> PermissionsList { get; set; }
    }
    public class ListPermissionViewModel
    {
        public ListPermissionViewModel()
        {
            this.PermissionList = new List<Permission>();
        }
        public ICollection<Permission> PermissionList { get; set; }
    }
    public class PermissionViewModel
    {
        public PermissionViewModel()
        {
        }
        public PermissionViewModel(Permission per)
        {
            Id = per.Id;
            ControllerKey = per.ControllerKey;
            AcctionKey = per.AcctionKey;
            Description = per.Description;
        }
        public PermissionViewModel(Permission per,bool selected):this(per)
        {
            Selected = selected;
        }
        public Permission Permission
        {
            set
            {
                Id = value.Id;
                ControllerKey = value.ControllerKey;
                AcctionKey = value.AcctionKey;
                Description = value.Description;
            }
        }
        [Required(AllowEmptyStrings = false)]
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string ControllerKey { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string AcctionKey { get; set; }
        public string Description { get; set; }
        public bool Selected { get; set; }
    }

    public class GroupViewModel
    {
        public GroupViewModel()
        {
            this.AllUsersInGroupList = new List<UserGroup>();
            this.PermissionsList = new List<PermissionViewModel>();
        }
        public GroupViewModel(Group grp):this()
        {
            Id = grp.Id;
            Name = grp.Name;
            Description = grp.Description;
            ParentId = grp.ParentId;
        }
        [Required(AllowEmptyStrings = false)]
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string ParentId { get; set; }
        public IEnumerable<UserGroup> AllUsersInGroupList { get; set; }
        public IEnumerable<PermissionViewModel> PermissionsList { get; set; }
        public IEnumerable<Permission> AllPermissionsInGroupList { get; set; }
    }
}