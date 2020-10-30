using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SSD.SSO.Sample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //How do I solve an AntiForgeryToken exception that occurs after an iisreset in my ASP.Net MVC app?
            //System.Web.Helpers.AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //http://stackoverflow.com/questions/30976980/mvc-5-owin-login-with-claims-and-antiforgerytoken-do-i-miss-a-claimsidentity-pr
            //Your claim identity does not have ClaimTypes.NameIdentifier, you should add more into claim array:
            //var claims = new List<Claim>
            //{
            //    new Claim(ClaimTypes.Name, "Brock"),
            //    new Claim(ClaimTypes.Email, "brockallen@gmail.com"),
            //    new Claim(ClaimTypes.NameIdentifier, "userId"), //should be userid with IdentityServer3
            //};

            //OR
            //http://stackoverflow.com/questions/19977833/anti-forgery-token-issue-mvc-5
            //Change to Subject

            AntiForgeryConfig.UniqueClaimTypeIdentifier = SSD.Framework.UGConstants.ClaimTypes.Subject;
        }
    }
}
