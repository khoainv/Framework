using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSD.SSO.Identity
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() : base()
        {
            //Stores = new List<int>();
            Permissions = new List<ApplicationPermission>();
            ListRoles = new List<string>();
        }
        public ApplicationUser(string username)
            : base(username)
        {
            //Stores = new List<int>();
            Permissions = new List<ApplicationPermission>();
            ListRoles = new List<string>();
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public bool IsSystemAccount { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Contry { get; set; }
        //public string StoresView { get; set; }
        //public int CompaniesView { get; set; }
        // Concatenate the address info for display in tables and such:
        public string DisplayAddress
        {
            get
            {
                string dspAddress = string.IsNullOrWhiteSpace(this.Address) ? "" : this.Address;
                string dspCity = string.IsNullOrWhiteSpace(this.City) ? "" : this.City;
                string dspContry = string.IsNullOrWhiteSpace(this.Contry) ? "" : this.Contry;
                return string.Format("{0}, {1}, {2}", dspAddress, dspCity, dspContry);
            }
        }
        [NotMapped]
        public List<ApplicationPermission> Permissions { get; set; }
        [NotMapped]
        public IList<string> ListRoles { get; set; }
        [NotMapped]
        public IList<IdentityUserRole> CacheRolesID { get; set; }

        public bool HasPermission(string accKey)
        {
            if (Permissions != null && Permissions.Any())
            {
                return Permissions.Where(x => x.AcctionKey == accKey).Count() > 0;
            }
            return false;
        }
    }
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base()
        {
            this.ApplicationPermissions = new List<ApplicationPermissionRole>();
        }
        public ApplicationRole(string name) : base(name) { }
        public ApplicationRole(string name, string decription) : base(name) { Description = decription; }
        public string Description { get; set; }

        public virtual ICollection<ApplicationPermissionRole> ApplicationPermissions { get; set; }
    }

    public class ApplicationPermission
    {
        public ApplicationPermission()
        {
            this.Id = System.Guid.NewGuid().ToString();
            this.ApplicationRoles = new List<ApplicationPermissionRole>();
        }
        public ApplicationPermission(string controllerKey, string acctionKey)
            : this()
        {
            this.ControllerKey = controllerKey;
            this.AcctionKey = acctionKey;

        }
        public ApplicationPermission(string controllerKey, string acctionKey, string description)
            : this(controllerKey, acctionKey)
        {
            this.Description = description;
        }
        [System.ComponentModel.DataAnnotations.Key]
        public string Id { get; set; }
        public string ControllerKey { get; set; }
        public string AcctionKey { get; set; }
        public string Description { get; set; }
        public virtual ICollection<ApplicationPermissionRole> ApplicationRoles { get; set; }
    }
    public class ApplicationPermissionRole
    {
        public string ApplicationPermissionId { get; set; }
        public string ApplicationRoleId { get; set; }
    }

}