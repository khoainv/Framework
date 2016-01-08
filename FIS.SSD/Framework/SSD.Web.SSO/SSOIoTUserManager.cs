using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using SSD.Framework;
using SSD.Framework.Extensions;
using SSD.Web.Identity;
using SSD.Web.Caching;
using SSD.Web.Security;
using SSD.Web.Models;
using Microsoft.AspNet.Identity;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using SSD.Framework.SSOClient;
using System.Threading.Tasks;
using System.Security.Claims;
using SSD.Framework.Security;

namespace SSD.Web.SSO
{
    public partial class SSOIoTUserManager : IoTUserManagerBase
    {
        public static SSOIoTUserManager Create(IdentityFactoryOptions<SSOIoTUserManager> options, IOwinContext context)
        {
            return new SSOIoTUserManager();
        }
        public SSOIoTUserManager() : base(HttpContext.Current.GetOwinContext().Get<SSOGroupManager>(),
            HttpContext.Current.GetOwinContext().Get<SSOPermissionManager>())
        {

        }
        public GroupManagerBase GroupManager
        {
            get
            {
                if (_grpManager == null)
                    _grpManager = HttpContext.Current.GetOwinContext().Get<SSOGroupManager>();
                return _grpManager;
            }
        }
        public PermissionManagerBase PermissionManager
        {
            get
            {
                if (_perManager == null)
                    _perManager = HttpContext.Current.GetOwinContext().Get<SSOPermissionManager>();
                return _perManager;
            }
        }
        private SSOUserManager _userManager;
        public SSOUserManager UserManager
        {
            get
            {
                if (_userManager == null)
                    _userManager = HttpContext.Current.GetOwinContext().Get<SSOUserManager>();
                return _userManager;
            }
        }
        public override User GetUserCache(string userName)
        {
            string cacheKey = GetCacheKey(userName);
            if (CacheUser.Contain(cacheKey))
            {
                return CacheUser.Get<User>(cacheKey);
            }
            else
            {
                var userDB = UserManager.UGUsers.Where(x => x.UserName == userName);
                //Check User
                if (userDB.Count() > 0)
                {
                    var user = userDB.First();
                    return SetUserToCache(user, cacheKey);
                }
                else if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return new User() { UserName = userName };
                }
            }
            return null;
        }
        public override bool IsValidUser(UserAuthen user)
        {
            return !string.IsNullOrWhiteSpace(GetAccessToken(user));
        }
        public string GetAccessToken(UserAuthen user)
        {
            var result = RequestToken(user);
            return result.AccessToken;
        }
        public TokenResponse RequestToken(UserAuthen user)
        {
            string urlToken = UGConstants.SSO.TokenEndpoint;
            var client = new OAuth2Client(new Uri(urlToken)
                , UGConstants.SSOClient.ClientId
                , UGConstants.SSOClient.ClientSecret);

            return client.RequestResourceOwnerPasswordAsync(user.UserName, user.Password, "openid profile email").Result;//write
        }

        public async Task<User> GetBySSOIoTUserAsync(string accessToken)
        {
            var client = new UserInfoClient(
                new Uri(UGConstants.SSO.UserInfoEndpoint),
                accessToken);

            var response = await client.GetAsync();
            var user = new User();
            if (response.Claims != null)
            {
                foreach (var ui in response.Claims)
                {
                    if (ui.Item1 == UGConstants.ClaimTypes.Subject)
                        user.Id = ui.Item2;
                    if (ui.Item1 == UGConstants.ClaimTypes.PreferredUserName)
                        user.UserName = ui.Item2;
                }
            }
            return user;
        }
    }
}
