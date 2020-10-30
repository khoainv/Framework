using Owin;

namespace SSD.Web.SSO
{
    public class SSOFactory
    {
        public static void CreateOwinContext(IAppBuilder app)
        {
            app.CreatePerOwinContext(SSODbContext.Create);
            app.CreatePerOwinContext<SSOPermissionManager>(SSOPermissionManager.Create);
            app.CreatePerOwinContext<SSOGroupManager>(SSOGroupManager.Create);
            app.CreatePerOwinContext<SSOUserManager>(SSOUserManager.Create);
            app.CreatePerOwinContext<SSOIoTClientManager>(SSOIoTClientManager.Create);
            app.CreatePerOwinContext<SSOIoTUserManager>(SSOIoTUserManager.Create);
            app.CreatePerOwinContext<SSOAppConfigManager>(SSOAppConfigManager.Create);
        }
    }
}