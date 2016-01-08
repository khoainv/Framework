using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
namespace SSD.SSO.SampleAPIs
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        //protected void Application_BeginRequest(object sender, System.EventArgs e)
        //{
        //    System.Web.HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
        //}
        //Using Session
        //protected void Application_PostAuthorizeRequest()
        //{
        //    // WebApi SessionState
        //    var routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(HttpContext.Current));
        //    if (routeData != null && routeData.RouteHandler is System.Web.Http.WebHost.HttpControllerRouteHandler)
        //        HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
        //}
    }
}