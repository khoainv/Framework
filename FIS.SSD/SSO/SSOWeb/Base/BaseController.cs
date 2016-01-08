using Microsoft.Owin;
using SSOWeb.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using SSD.Framework.Extensions;
using SSD.SSO.Extensions;

namespace SSOWeb.Base
{
    [Serializable]
    public class SSOIdentity
    {
        public SSOIdentity() { }
        public SSOIdentity(IIdentity identity) { 
            AuthenticationType = identity.AuthenticationType;
            IsAuthenticated = identity.IsAuthenticated;
            Name = identity.Name;
        }
        public SSOIdentity(IPrincipal user)
        {
            AuthenticationType = user.Identity.AuthenticationType;
            IsAuthenticated = user.Identity.IsAuthenticated;
            Name = user.GetUserName();
        }
        
        public string AuthenticationType { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Name { get; set; }
    }
    public class UGAuthorizeAttribute : Attribute
    {
        public string Key { get; set; }
        public string Name { get; set; }
    }
    public class UGFollowAuthorizeAttribute : Attribute
    {
        public string FollowKey { get; set; }
    }
    public class BaseController : Controller
    {
        public new ClaimsPrincipal User
        {
            get { return base.User as ClaimsPrincipal; }
        }

        public WorkContext WorkContext { get; set; }
        public IOwinContext OwinContext
        {
            get
            {
                return HttpContext.GetOwinContext();
            }
        }
        private System.Web.Routing.RouteValueDictionary RouteAccessDenied(string msg)
        {
            return new System.Web.Routing.RouteValueDictionary(new { controller = "Home",  action = "AccessDenied" ,
                                             area = string.Empty, message = msg });
        }
        private System.Web.Routing.RouteValueDictionary RouteLogin(string RawUrl)
        {
            return new System.Web.Routing.RouteValueDictionary(new
            {
                controller = "Account",
                action = "Login",
                area = string.Empty,
                ReturnUrl = RawUrl
            });
        }
        protected override void OnAuthentication(System.Web.Mvc.Filters.AuthenticationContext filterContext)
        {
            filterContext.CheckNull("AuthorizationContext is not null");
            base.OnAuthentication(filterContext);
            IPrincipal user = filterContext.HttpContext.User;

            var lstAttCtr = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(UGAuthorizeAttribute), false);
            var lstAttAcction = filterContext.ActionDescriptor.GetCustomAttributes(typeof(UGAuthorizeAttribute), false);
            var lstAttAjaxAcction = filterContext.ActionDescriptor.GetCustomAttributes(typeof(UGFollowAuthorizeAttribute), false);
            
            if (user.Identity.IsAuthenticated)
            {
                WorkContext = new WorkContext(user.GetUserName(), filterContext);
                if (!WorkContext.IsInited(user.Identity.Name))
                {
                    WorkContext.Init();
                    if(WorkContext.CurrentUser==null)
                    {
                        OwinContext.Authentication.SignOut();
                        WorkContext = null;
                        //redirect to Loginform
                        filterContext.Result = new RedirectToRouteResult(RouteLogin(filterContext.HttpContext.Request.RawUrl));
                    }
                }
                var attCtrKey = lstAttCtr.Length > 0 ? (lstAttCtr.First() as UGAuthorizeAttribute).Key : "";
                var attCtrName = lstAttCtr.Length > 0 ? (lstAttCtr.First() as UGAuthorizeAttribute).Name : "";
                if (lstAttAcction.Length > 0)
                {
                    UGAuthorizeAttribute att = lstAttAcction.First() as UGAuthorizeAttribute;
                    //Check permission
                    if (!WorkContext.CurrentUser.HasPermission(att.Key))
                    {
                        //redirect to Accecc Diney
                        filterContext.Result = new RedirectToRouteResult(RouteAccessDenied(attCtrName+"|"+att.Name));
                    }
                }
                else if (lstAttAjaxAcction.Length > 0)
                {
                    UGFollowAuthorizeAttribute att = lstAttAjaxAcction.First() as UGFollowAuthorizeAttribute;
                    //Check permission
                    if (!WorkContext.CurrentUser.HasPermission(att.FollowKey))
                    {
                        //redirect to Accecc Diney
                        filterContext.Result = new RedirectToRouteResult(RouteAccessDenied(string.Format("Bạn không có quyền vào chức năng: 'Controller|{0}-Acction|{1}'", attCtrKey, att.FollowKey)));
                    }
                }
            }
            else
            {
                if (lstAttAcction.Length > 0)
                {
                    //redirect to Loginform
                    filterContext.Result = new RedirectToRouteResult(RouteLogin(filterContext.HttpContext.Request.RawUrl));
                }
            }

        }

    }
}