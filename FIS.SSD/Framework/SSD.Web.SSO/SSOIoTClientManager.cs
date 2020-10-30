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
    public class SSOIoTClientManager : IoTClientManagerBase
    {
        public SSOIoTClientManager() : base(HttpContext.Current.GetOwinContext().Get<SSODbContext>())
        {
        }
        public SSOIoTClientManager(SSODbContext context) : base(context)
        {
        }
        public static SSOIoTClientManager Create(IdentityFactoryOptions<SSOIoTClientManager> options, IOwinContext context)
        {
            return new SSOIoTClientManager(context.Get<SSODbContext>());
        }
    }
}