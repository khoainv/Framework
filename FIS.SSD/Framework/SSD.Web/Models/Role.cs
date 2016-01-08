using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSD.Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class Role : IdentityRole
    {
        public Role() : base()
        {
        }
        public Role(string name) : base(name) { }
        public Role(string name, string decription) : base(name) { Description = decription; }
        [StringLength(256)]
        public string Description { get; set; }
    }
}