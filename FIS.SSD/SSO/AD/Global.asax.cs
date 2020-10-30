using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;

namespace ADIdentityServer
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
