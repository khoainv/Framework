using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using System;
using Microsoft.AspNet.Identity.Owin;
using SSD.Framework.Extensions;
using System.Web;
using SSD.SSO.Identity;
using SSD.SSO.Caching;

namespace SSOWeb.Base
{
    public class WorkContext
    {
        private IOwinContext _owinContext;
        public IOwinContext OwinContext
        {
            get
            {
                return _owinContext;
            }
        }

        private string _userName;
        private Lazy<ApplicationUser> currentUser;
        const string CacheUserKeyPrefix = "UGSSO";
        public static bool IsInited(string userName)
        {
            return CacheUser.Contain(CacheUserKeyPrefix + userName);
        }
        public static void DeleteUser(string userName)
        {
            CacheUser.Remove(CacheUserKeyPrefix + userName);
        }
        public void ClearCache()
        {
            CacheUser.Flush();
        }
        public ApplicationUser CurrentUser
        {
            get
            {
                var cachedUser = CacheUser.Get<ApplicationUser>(CacheUserKeyPrefix + this._userName);
                if (cachedUser != null)
                {
                    return cachedUser;
                }
                else
                {
                    if (currentUser == null)
                        UpdateCurrentUser(this._userName);
                    var user = currentUser.Value;
                    CacheUser.Set(CacheUserKeyPrefix + this._userName, user);
                    return user;
                }
            }
        }
        public void UpdateCurrentUser(string userName)
        {
            var manager = OwinContext.GetUserManager<ApplicationUserManager>();
            this.currentUser = new Lazy<ApplicationUser>(() => manager.FindByName(userName));
            var user = currentUser.Value;
            //Add Permision, Profile to User...
            if (user != null)
            {
                var roleManager = OwinContext.GetUserManager<ApplicationRoleManager>();
                user.ListRoles = manager.GetRoles(user.Id);
                user.CacheRolesID = user.Roles.ToList();

                var perManager = OwinContext.GetUserManager<ApplicationPermissionManager>();
                user.Permissions = perManager.GetUserPermissions(user.Id).ToList();
            }
            CacheUser.Set(CacheUserKeyPrefix + userName, user);
        }
        public WorkContext(string userName, IOwinContext owinContext)
        {
            userName.CheckNull("email is not null");

            this._userName = userName;
            this._owinContext = owinContext;

        }
        public WorkContext(string userName, System.Web.Mvc.Filters.AuthenticationContext filterContext)
        {
            userName.CheckNull("email is not null");

            this._userName = userName;
            this._owinContext = filterContext.HttpContext.GetOwinContext();

        }
        public void Init()
        {
            UpdateCurrentUser(this._userName);
        }
    }
}