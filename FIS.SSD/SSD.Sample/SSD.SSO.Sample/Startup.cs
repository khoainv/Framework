using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SSD.SSO.Sample.Startup))]
namespace SSD.SSO.Sample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
