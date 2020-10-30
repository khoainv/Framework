using Microsoft.Owin;
using Owin;
using SSD.Web.Identity;

[assembly: OwinStartupAttribute(typeof(SSD.SampleAPIs.Startup))]
namespace SSD.SampleAPIs
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            IdentityFactory.CreateOwinContext(app);
        }
    }
}
