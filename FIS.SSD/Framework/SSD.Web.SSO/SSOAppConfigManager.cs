using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using SSD.Framework;
using SSD.Web.Identity;
using SSD.Web.Models;

namespace SSD.Web.SSO
{
    public class SSOAppConfigManager : AppConfigManagerBase
    {
        public SSOAppConfigManager() : base(HttpContext.Current.GetOwinContext().Get<SSODbContext>())
        {
        }
        public SSOAppConfigManager(SSODbContext context) : base(context)
        {
        }
        public static SSOAppConfigManager Create(IdentityFactoryOptions<SSOAppConfigManager> options, IOwinContext context)
        {
            return new SSOAppConfigManager(context.Get<SSODbContext>());
        }
    }
}