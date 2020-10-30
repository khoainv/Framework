using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSD.Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class Permission
    {
        public Permission()
        {
            this.Id = System.Guid.NewGuid().ToString();
            this.Groups = new List<PermissionGroup>();
        }
        public Permission(string controllerKey, string acctionKey)
            : this()
        {
            this.ControllerKey = controllerKey;
            this.AcctionKey = acctionKey;

        }
        public Permission(string controllerKey, string acctionKey, string description)
            : this(controllerKey, acctionKey)
        {
            this.Description = description;
        }
        [Key]
        [StringLength(36)]
        public string Id { get; set; }
        [StringLength(256)]
        [Required]
        public string ControllerKey { get; set; }
        [StringLength(256)]
        [Required]
        public string AcctionKey { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public virtual ICollection<PermissionGroup> Groups { get; set; }

        public bool CompareTo(Permission per)
        {
            return ControllerKey == per.ControllerKey && AcctionKey == per.AcctionKey;
        }
    }
}