using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using System;
using Microsoft.AspNet.Identity.Owin;
using SSD.Framework;
using System.Web;
using SSD.Web.Caching;
using SSD.Web.Models;
using SSD.Web.Identity;

namespace SSD.Web.Mvc.Controllers
{
    public class WorkContext: WorkContextBase
    {
        protected override void UpdateUser(string userName)
        {
            var manager = OwinContext.GetUserManager<UserManager>();
            this.currentUser = new Lazy<User>(() => manager.FindByName(userName));
            var user = currentUser.Value;
            //Add Permision, Profile to User...
            if (user != null)
            {
                var roleManager = OwinContext.Get<RoleManager>();
                user.ListRoles = manager.GetRoles(user.Id);
                user.CacheRolesID = user.Roles.ToList();

                var grpManager = OwinContext.GetUserManager<GroupManager>();
                user.Groups = grpManager.FindByUserName(user.UserName).ToList();

                var perManager = OwinContext.Get<PermissionManager>();
                user.Permissions = perManager.FindPermissionsByUserName(user.UserName).ToList();

                user.JsonProfile = _jsonProfile;
            }
            CacheUser.Set(CacheUserKeyPrefix + userName, user);
        }
       
        public WorkContext(string userName, IOwinContext owinContext):base(userName, owinContext)
        {
        }
        public WorkContext(System.Web.Mvc.Filters.AuthenticationContext filterContext) : base(filterContext)
        {
        }
    }
}