using Microsoft.AspNet.Identity;
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
    public class SSOGroupManager : GroupManagerBase
    {
        public SSOGroupManager() : base(HttpContext.Current.GetOwinContext().Get<SSODbContext>())
        {
        }
        public SSOGroupManager(SSODbContext context) : base(context)
        {
        }
        public static SSOGroupManager Create(IdentityFactoryOptions<SSOGroupManager> options, IOwinContext context)
        {
            return new SSOGroupManager(context.Get<SSODbContext>());
        }
    }
}