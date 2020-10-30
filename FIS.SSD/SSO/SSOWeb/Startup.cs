using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SSOWeb.Startup))]
namespace SSOWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureIdentityServer3(app);

            ConfigureAuth(app);
        }
    }
}
