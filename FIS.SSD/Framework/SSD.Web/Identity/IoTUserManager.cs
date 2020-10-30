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
using System.Threading.Tasks;
using Microsoft.Owin;
using SSD.Framework.Security;

namespace SSD.Web.Identity
{
    public partial class IoTUserManager : IoTUserManagerBase
    {
        public static IoTUserManager Create(IdentityFactoryOptions<IoTUserManager> options, IOwinContext context)
        {
            return new IoTUserManager();
        }
        public IoTUserManager() : base(HttpContext.Current.GetOwinContext().Get<GroupManager>(),
            HttpContext.Current.GetOwinContext().Get<PermissionManager>())
        {

        }
        public GroupManagerBase GroupManager
        {
            get
            {
                if (_grpManager == null)
                    _grpManager = HttpContext.Current.GetOwinContext().Get<GroupManager>();
                return _grpManager;
            }
        }
        public PermissionManagerBase PermissionManager
        {
            get
            {
                if (_perManager == null)
                    _perManager = HttpContext.Current.GetOwinContext().Get<PermissionManager>();
                return _perManager;
            }
        }
        private UserManager _userManager;
        public UserManager UserManager
        {
            get
            {
                if (_userManager == null)
                    _userManager = HttpContext.Current.GetOwinContext().Get<UserManager>();
                return _userManager;
            }
        }
        #region API
        //Kiem tra cac lan tiep theo
        public override bool IsValidUser(UserAuthen user)
        {
            return IsValidUser(user, true);
        }
        //Login lan dau
        public bool IsValidUser(UserAuthen user, bool isLogined)
        {
            //Check User
            bool isValid = false;
            string cacheKey = GetCacheKey(user.UserName);
            if (CacheUser.Contain(cacheKey))
            {
                var uc = CacheUser.Get<User>(cacheKey);

                if (isLogined)
                {
                    //Fast check
                    isValid = uc.PasswordHash == user.PasswordHash;
                }
                else
                {
                    //Very slow
                    var passHash = new CustomPasswordHasher();
                    isValid = passHash.VerifyHashedPassword(uc.PasswordHash, user.Password) == PasswordVerificationResult.Success;
                    if (isValid)
                        user.PasswordHash = uc.PasswordHash;
                }
            }
            else
            {
                //Check User
                var userDB = UserManager.Find(user.UserName, user.Password);
                //Check User
                if (userDB != null)
                {
                    SetUserToCache(userDB, cacheKey);
                    user.PasswordHash = userDB.PasswordHash;
                    isValid = true;
                }
            }
            return isValid;
        }
        
        public User GetUser(UserAuthen user)
        {
            string cacheKey = GetCacheKey(user.UserName);
            if (CacheUser.Contain(cacheKey))
            {
                return CacheUser.Get<User>(cacheKey);
            }
            else
            {
                var userDB = UserManager.FindByName(user.UserName);
                //Check User
                if (userDB != null)
                {
                    return SetUserToCache(userDB, cacheKey);
                }
            }
            return null;
        }
        public User GetUser(UGToken token)
        {
            return GetUser(new UserAuthen(token));
        }
        #endregion

        public override User GetUserCache(string userName)
        {
            return GetUser(new UserAuthen() { UserName = userName });
        }
    }
    public abstract class IoTUserManagerBase : BaseManager, ICacheManager
    {
        public abstract bool IsValidUser(UserAuthen user);
        public abstract User GetUserCache(string userName);
        protected GroupManagerBase _grpManager;
        protected PermissionManagerBase _perManager;
        const string CacheUserKeyPrefix = "api_";
        private List<Permission> _lstPermission;
        public List<Permission> Permissions
        {
            get
            {
                if (_lstPermission == null)
                    _lstPermission = _perManager.Permissions.ToList();
                return _lstPermission;
            }
        }
        protected string GetCacheKey(string userName)
        {
            return CacheUserKeyPrefix + userName;
        }

        public IoTUserManagerBase(GroupManagerBase grpManager, PermissionManagerBase perManager)
        {
            _grpManager = grpManager;
            _perManager = perManager;
        }

        public void CleanCache()
        {
            _grpManager.CleanCache();
            _perManager.CleanCache();
            CacheUser.Flush();
        }

        public async Task UpdateCacheAsync()
        {
            CleanCache();
            await _grpManager.UpdateCacheAsync();
            await _perManager.UpdateCacheAsync();
        }
        public void ClearCacheUser(string userName)
        {
            string cacheKey = GetCacheKey(userName);
            CacheUser.Remove(cacheKey);
        }
        protected User SetUserToCache(User userDB, string cacheKey)
        {
            //Groups of Application
            var lstGrp = _grpManager.FindByUserName(userDB.UserName).Distinct();
            userDB.Groups = new List<Group>(lstGrp);
            //Permission of Application
            var lstPer = _perManager.FindPermissionsByUserName(userDB.UserName).Distinct();
            userDB.Permissions = new List<Permission>(lstPer);

            //cache
            CacheUser.Set<User>(cacheKey, userDB);
            return userDB;
        }
        public void UpdateProfile(string userName, string strProfile)
        {
            string cacheKey = GetCacheKey(userName);
            if (CacheUser.Contain(cacheKey))
            {
                var uc = CacheUser.Get<User>(cacheKey);
                uc.JsonProfile = strProfile;
                CacheUser.Set<User>(cacheKey, uc);
            }
        }
    }
}
