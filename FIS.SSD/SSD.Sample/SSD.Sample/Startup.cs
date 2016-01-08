using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SSD.Sample.Startup))]
namespace SSD.Sample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
