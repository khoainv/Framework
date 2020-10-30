using System.Linq;
using Microsoft.Owin;
using System;
using SSD.Framework.Extensions;
using System.Web;
using SSD.Web.Caching;
using SSD.Web.Models;
using SSD.Web.Extensions;

namespace SSD.Web.Mvc.Controllers
{
    public abstract class WorkContextBase
    {
        #region Constructor
        public WorkContextBase(string userName, IOwinContext owinContext)
        {
            userName.CheckNull("userName is not null");

            this._userName = userName;
            this._owinContext = owinContext;
        }
        public WorkContextBase(System.Web.Mvc.Filters.AuthenticationContext filterContext)
        {
            if (filterContext.HttpContext.User == null)
                throw new Exception("user is not null");
            string userName = filterContext.HttpContext.User.GetUserName();
            this._userName = userName;
            this._owinContext = filterContext.HttpContext.GetOwinContext();
        }
        #endregion

        #region Protected
        protected abstract void UpdateUser(string userName);
        protected IOwinContext _owinContext;
        public IOwinContext OwinContext
        {
            get
            {
                return _owinContext;
            }
        }

        protected string _userName;
        internal void InitUser()
        {
            UpdateUser(this._userName);
        }
        protected const string CacheUserKeyPrefix = "Core";
       
        protected Lazy<User> currentUser;
        protected string _jsonProfile;
        protected T GetProfile<T>() where T : class, new()
        {
            if (CurrentUser != null)
            {
                var user = CurrentUser;
                if (string.IsNullOrWhiteSpace(user.JsonProfile))
                    return default(T);
                return user.JsonProfile.DeserializeJson<T>();
            }
            return default(T);
        }
        #endregion
        public bool IsInitedUser(string userName)
        {
            return CacheUser.Contain(CacheUserKeyPrefix + userName);
        }
        public void SetProfile<T>(T profile) where T : class, new()
        {
            _jsonProfile = profile.SerializeJson();
        }
        protected string Key
        {
            get
            {
                return CacheUserKeyPrefix + this._userName;
            }
        }
        public static void DeleteUser(string userName)
        {
            CacheUser.Remove(CacheUserKeyPrefix + userName);
        }
        public void UpdateCurrentUser()
        {
            if (!string.IsNullOrWhiteSpace(_userName))
                UpdateUser(_userName);
        }
        public static void ClearCache()
        {
            CacheUser.Flush();
        }
        public User CurrentUser
        {
            get
            {
                var cachedUser = CacheUser.Get<User>(Key);
                if (cachedUser != null)
                {
                    return cachedUser;
                }
                else
                {
                    if (currentUser == null)
                        UpdateUser(this._userName);
                    var user = currentUser.Value;
                    CacheUser.Set(Key, user);
                    return user;
                }
            }
        }

        public string UserName
        {
            get { return _userName; }
        }
    }
}