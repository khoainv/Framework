using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using SSD.Web.Identity;
using SSD.Web.Models;

namespace SSD.Web.SSO
{
    public class SSOPermissionManager : PermissionManagerBase
    {
        public SSOPermissionManager():base(HttpContext.Current.GetOwinContext().Get<SSODbContext>())
        {
        }
        public SSOPermissionManager(SSODbContext context) : base(context)
        {
        }
        public static SSOPermissionManager Create(IdentityFactoryOptions<SSOPermissionManager> options, IOwinContext context)
        {
            return new SSOPermissionManager(context.Get<SSODbContext>());
        }
    }
}