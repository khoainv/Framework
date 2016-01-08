using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using SSD.Web.Models;

namespace SSD.Web.Identity
{
    // Configure the RoleManager used in the application. RoleManager is defined in the ASP.NET Identity core assembly
    public class RoleManager : RoleManager<Role>
    {
        public RoleManager(IdentityDbContext context)
            : base(new RoleStore<Role>(context))
        {
        }
        public static RoleManager Create(IdentityFactoryOptions<RoleManager> options, IOwinContext context)
        {
            return new RoleManager(context.Get<IdentityDbContext>());
        }
    }
}
