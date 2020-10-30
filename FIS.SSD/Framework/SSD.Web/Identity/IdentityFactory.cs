using System;
using Owin;
using SSD.Web.Identity;

namespace SSD.Web.Identity
{
    public class IdentityFactory
    {
        public static void CreateOwinContext(IAppBuilder app)
        {
            app.CreatePerOwinContext(IdentityDbContext.Create);
            app.CreatePerOwinContext<UserManager>(UserManager.Create);
            app.CreatePerOwinContext<RoleManager>(RoleManager.Create);
            app.CreatePerOwinContext<PermissionManager>(PermissionManager.Create);
            app.CreatePerOwinContext<GroupManager>(GroupManager.Create);
            app.CreatePerOwinContext<IoTClientManager>(IoTClientManager.Create);
            app.CreatePerOwinContext<IoTUserManager>(IoTUserManager.Create);
            app.CreatePerOwinContext<AppConfigManager>(AppConfigManager.Create);
            app.CreatePerOwinContext<SignInManager>(SignInManager.Create);
        }
    }
}