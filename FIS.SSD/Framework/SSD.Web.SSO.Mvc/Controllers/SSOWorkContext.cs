using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using System;
using Microsoft.AspNet.Identity.Owin;
using SSD.Framework;
using System.Web;
using SSD.Web.Caching;
using SSD.Web.Models;
using SSD.Web.Extensions;
using System.Security.Principal;
using SSD.Web.Identity;
using SSD.Web.Mvc.Controllers;

namespace SSD.Web.SSO.Mvc.Controllers
{
    public class SSOWorkContext: WorkContextBase
    {
        public IPrincipal User { get; set; }
        protected override void UpdateUser(string userName)
        {
            var userManager = OwinContext.GetUserManager<SSOUserManager>();
            this.currentUser = new Lazy<User>(() => userManager.GetBySSOUser(User));
            var user = currentUser.Value;
            //Add Permision, Profile to User...
            if (user != null)
            {
                var grpManager = OwinContext.Get<SSOGroupManager>();
                user.Groups = grpManager.FindByUserName(user.UserName).ToList();

                if(user.Groups.Count==0)
                {
                    var grpAnonymous = grpManager.FindByName(UGConstants.GroupAnonymous);
                    grpManager.AddUserToGroup(user.UserName, grpAnonymous.Id);
                    user.Groups = grpManager.FindByUserName(user.UserName).ToList();
                }

                var perManager = OwinContext.Get<SSOPermissionManager>();
                user.Permissions = perManager.FindPermissionsByUserName(user.UserName).ToList();

                user.JsonProfile = _jsonProfile;
            }
            CacheUser.Set(CacheUserKeyPrefix + userName, user);
        }
        public SSOWorkContext(System.Web.Mvc.Filters.AuthenticationContext filterContext) 
            : base(filterContext)
        {
            User = filterContext.HttpContext.User;
        }
        public SSOWorkContext(IPrincipal user, IOwinContext context)
            : base(user.GetUserName(), context)
        {
            User = user;
        }
    }
}