using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSD.Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class PermissionGroup
    {
        [Key]
        [StringLength(36)]
        public string PermissionId { get; set; }
        [Key]
        [StringLength(36)]
        public string GroupId { get; set; }
    }

}