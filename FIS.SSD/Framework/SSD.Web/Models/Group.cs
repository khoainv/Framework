using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSD.Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class Group
    {
        public Group()
        {
            this.Id = System.Guid.NewGuid().ToString();
            this.Permissions = new List<PermissionGroup>();
            Users = new List<UserGroup>();
            Children = new List<Group>();
        }
        public Group(string name):this() {
            Name = name;
        }
        public Group(string name, string decription) : this(name)
        {
            Description = decription;
        }
        [Key]
        [StringLength(36)]
        public string Id { get; set; }

        [StringLength(100)]
        [Required]
        public string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [StringLength(36)]
        public string ParentId { get; set; }

        public virtual ICollection<PermissionGroup> Permissions { get; set; }
        [NotMapped]
        public virtual ICollection<UserGroup> Users { get; set; }
        [NotMapped]
        public virtual ICollection<Group> Children { get; set; }
    }

}