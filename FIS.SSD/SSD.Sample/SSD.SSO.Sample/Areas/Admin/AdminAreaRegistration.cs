
using System.Web.Mvc;

namespace SSD.SSO.Sample.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SSD.Web.SSO.Mvc.Controllers", "SSD.SSO.Sample.Areas.Admin.Controllers" }
            );
        }
    }
}