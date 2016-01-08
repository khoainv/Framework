using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSD.Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class User : IdentityUser
    {
        public User() : base()
        {
            Permissions = new List<Permission>();
            Groups = new List<Group>();
            ListRoles = new List<string>();
        }
        public User(string username)
            : base(username)
        {
            Permissions = new List<Permission>();
            Groups = new List<Group>();
            ListRoles = new List<string>();
        }
        public override string UserName { get; set; }
        public bool IsSystemAccount { get; set; }
        [StringLength(500)]
        public string Address { get; set; }
        [StringLength(256)]
        public string City { get; set; }
        [StringLength(100)]
        public string Contry { get; set; }
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
        public List<Group> Groups { get; set; }
        public bool IsGroup(string grpName)
        {
            if (Groups != null && Groups.Any())
            {
                return Groups.Where(x => x.Name == grpName).Count() > 0;
            }
            return false;
        }
        [NotMapped]
        public List<Permission> Permissions { get; set; }
        public bool HasPermission(string accKey)
        {
            if (Permissions != null && Permissions.Any())
            {
                return Permissions.Where(x => x.AcctionKey == accKey).Count() > 0;
            }
            return false;
        }
        [NotMapped]
        public IList<string> ListRoles { get; set; }
        [NotMapped]
        public IList<IdentityUserRole> CacheRolesID { get; set; }
        [NotMapped]
        public string JsonProfile { get; set; }
    }
}