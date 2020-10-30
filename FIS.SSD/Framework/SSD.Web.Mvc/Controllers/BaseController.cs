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
using SSD.Web.Security;
using SSD.Framework.Security;

namespace SSD.Web.Mvc.Controllers
{
    public class BaseController<T> : UGControllerBase where T : WorkContext
    {
        public virtual T WorkContext { get; set; }
        public override void InitWorkContext(System.Web.Mvc.Filters.AuthenticationContext filterContext)
        {
            if(WorkContext == null)
                WorkContext = new WorkContext(filterContext) as T;

            SetWorkContext(WorkContext);
        }
    }
    public abstract class UGControllerBase : Controller
    {
        public virtual string AccountControllerName { get; set; }
        public abstract void InitWorkContext(System.Web.Mvc.Filters.AuthenticationContext filterContext);
        public new ClaimsPrincipal User
        {
            get { return base.User as ClaimsPrincipal; }
        }

        private WorkContextBase _workContext;
        protected void SetWorkContext(WorkContextBase workContext)
        {
            _workContext = workContext;
        }
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
            string controller = string.IsNullOrWhiteSpace(AccountControllerName) ? "Account" : AccountControllerName;
            return new System.Web.Routing.RouteValueDictionary(new { controller = controller,  action = "Login" ,
                                             area = string.Empty, ReturnUrl = RawUrl } );
        }
        protected override void OnAuthentication(System.Web.Mvc.Filters.AuthenticationContext filterContext)
        {
            filterContext.CheckNull("AuthorizationContext is not null");
            base.OnAuthentication(filterContext);
            IPrincipal user = filterContext.HttpContext.User;

            var lstAttCtr = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(UGPermissionAttribute), false);
            var lstAttAcction = filterContext.ActionDescriptor.GetCustomAttributes(typeof(UGPermissionAttribute), false);
            var lstAttAjaxAcction = filterContext.ActionDescriptor.GetCustomAttributes(typeof(UGFollowPermissionAttribute), false);
            
            if (user.Identity.IsAuthenticated)
            {
                string userName = user.GetUserName();
                InitWorkContext(filterContext);
                //WorkContext = new T(user.Identity.Name, filterContext);
                if (!_workContext.IsInitedUser(userName))
                {
                    _workContext.InitUser();
                    var currentUser = _workContext.CurrentUser;
                    if (currentUser == null)
                    {
                        OwinContext.Authentication.SignOut();
                        _workContext = null;
                        //redirect to Loginform
                        filterContext.Result = new RedirectToRouteResult(RouteLogin(filterContext.HttpContext.Request.RawUrl));
                        return;
                    }
                }
                var userContext = _workContext.CurrentUser;
                var attCtrKey = lstAttCtr.Length > 0 ? (lstAttCtr.First() as UGPermissionAttribute).Key : "";
                var attCtrName = lstAttCtr.Length > 0 ? (lstAttCtr.First() as UGPermissionAttribute).Name : "";
                if (lstAttAcction.Length > 0)
                {
                    UGPermissionAttribute att = lstAttAcction.First() as UGPermissionAttribute;
                    //Check permission
                    if (!userContext.HasPermission(att.Key))
                    {
                        //redirect to Accecc Diney
                        filterContext.Result = new RedirectToRouteResult(RouteAccessDenied(attCtrName+"|"+att.Name));
                    }
                }
                else if (lstAttAjaxAcction.Length > 0)
                {
                    UGFollowPermissionAttribute att = lstAttAjaxAcction.First() as UGFollowPermissionAttribute;
                    //Check permission
                    if (!userContext.HasPermission(att.FollowKey))
                    {
                        //redirect to Accecc Diney
                        filterContext.Result = new RedirectToRouteResult(RouteAccessDenied(string.Format("Bạn không có quyền vào chức năng: 'Controller|{0}-Acction|{1}'", attCtrKey, att.FollowKey)));
                    }
                }
            }
            else
            {
                if(lstAttAcction.Length>0)
                {
                    //redirect to Loginform
                    filterContext.Result = new RedirectToRouteResult(RouteLogin(filterContext.HttpContext.Request.RawUrl));
                }
            }
        }
    }
    public class AdminAuthorizeAttribute : BaseAuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            filterContext.CheckNull("AuthorizationContext is not null");

            base.OnAuthorization(filterContext);
            var grpManager = filterContext.HttpContext.GetOwinContext().Get<GroupManager>();
            BaseAuthorization(filterContext, grpManager);
        }
    }
    public class BaseAuthorizeAttribute : AuthorizeAttribute
    {
        public string Groups { get; set; }
        private System.Web.Routing.RouteValueDictionary RouteAccessDenied(string msg)
        {
            return new System.Web.Routing.RouteValueDictionary(new
            {
                controller = "Home",
                action = "AccessDenied",
                area = string.Empty,
                message = msg
            });
        }
        private System.Web.Routing.RouteValueDictionary RouteLogin(string loginController, string RawUrl)
        {
            string controller = string.IsNullOrWhiteSpace(loginController) ? "Account" : loginController;
            return new System.Web.Routing.RouteValueDictionary(new
            {
                controller = controller,
                action = "Login",
                area = string.Empty,
                ReturnUrl = RawUrl
            });
        }
        protected void BaseAuthorization(AuthorizationContext filterContext, GroupManagerBase grpManager, string loginController= "Account")
        {
            IPrincipal user = filterContext.HttpContext.User;
            if (user.Identity.IsAuthenticated)
            {
                string userName = user.GetUserName();
                //var ctrl = filterContext.Controller as BaseController;
                //ctrl.WorkContext = new WorkContext(user.Identity.Name, filterContext);
                if (!string.IsNullOrWhiteSpace(Groups))
                {
                    var lstGroup = Groups.Split(new char[] { ';' });

                    var grps = grpManager.FindByUserName(userName).Where(x => lstGroup.Contains(x.Name));
                    if (grps.Count() == 0)
                    {
                        var actionDescriptor = filterContext.ActionDescriptor;
                        var currentAction = actionDescriptor.ActionName;
                        var currentController = actionDescriptor.ControllerDescriptor.ControllerName;
                        //redirect to Accecc Diney
                        filterContext.Result = new RedirectToRouteResult(RouteAccessDenied(currentController + "|" + currentAction));
                    }
                }
            }
            else
            {
                //redirect to Loginform
                filterContext.Result = new RedirectToRouteResult(RouteLogin(loginController,filterContext.HttpContext.Request.RawUrl));
            }
        }
    }
}