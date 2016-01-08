using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SSD.Web.Mvc;

namespace MvcAppTest
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            ViewEngines.Engines.Clear();
            //ViewEngines.Engines.AddIPhone<WebFormViewEngine>();
            //ViewEngines.Engines.AddGenericMobile<WebFormViewEngine>();
            //ViewEngines.Engines.AddWP7<RazorViewEngine>();
            //ViewEngines.Engines.AddIPad<RazorViewEngine>();
            //ViewEngines.Engines.AddIPhone<RazorViewEngine>();
            ViewEngines.Engines.AddGenericMobile<RazorViewEngine>();
            ViewEngines.Engines.AddGenericTablet<RazorViewEngine>();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }
    }
}