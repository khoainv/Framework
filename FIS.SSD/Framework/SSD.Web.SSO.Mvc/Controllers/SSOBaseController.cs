using Microsoft.Owin;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using SSD.Framework;
using SSD.Web.Extensions;
using SSD.Web.Identity;
using Microsoft.AspNet.Identity.Owin;
using SSD.Framework.Extensions;
using SSD.Web.Mvc.Controllers;

namespace SSD.Web.SSO.Mvc.Controllers
{
    public class SSOBaseController<T> : UGControllerBase where T : SSOWorkContext
    {
        private const string account = "SSOAccount";
        public override string AccountControllerName { get { return account; } }
        public virtual T WorkContext { get; set; }
        public override void InitWorkContext(System.Web.Mvc.Filters.AuthenticationContext filterContext)
        {
            if(WorkContext==null)
                WorkContext = new SSOWorkContext(filterContext) as T;
            SetWorkContext(WorkContext);
        }
    }
    public class SSOAutshorizeAttribute : BaseAuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            filterContext.CheckNull("AuthorizationContext is not null");

            base.OnAuthorization(filterContext);
            var grpManager = filterContext.HttpContext.GetOwinContext().Get<SSOGroupManager>();
            BaseAuthorization(filterContext, grpManager,"SSOAccount");
        }
    }
}