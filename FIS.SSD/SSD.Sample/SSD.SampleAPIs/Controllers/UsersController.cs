using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using SSD.Web.Security;
using SSD.Framework.Extensions;
using SSD.Web.Identity;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using SSD.Framework.Security;

namespace SSD.SampleAPIs.Controllers
{
    
    public class UsersController : ApiController
    {
        private IoTUserManager _iotUserManager;
        public IoTUserManager IoTUserMrg
        {
            get
            {
                if (_iotUserManager == null)
                    _iotUserManager = HttpContext.Current.GetOwinContext().Get<IoTUserManager>();
                return _iotUserManager;
            }
        }
        [HttpGet]
        public string ClearCacheTest()
        {
            IoTUserMrg.CleanCache();
            return "Clear Successfull.";
        }
        [HttpPost]
        public string ClearCache(UserAuthen user)
        {
            if (IoTUserMrg.IsValidUser(user))
            {
                IoTUserMrg.CleanCache();
            }
            return "Clear Successfull.";
        }
        [HttpPost]
        public string ClearCacheUser(UserAuthen user)
        {
            if (IoTUserMrg.IsValidUser(user))
            {
                IoTUserMrg.ClearCacheUser(user.UserName);
                return "Clear Successfull.";
            }
            return "Not Permission.";
        }
        [HttpPost]
        public LoginResult Authenticate(UserAuthen user)
        {
            if (user == null)
                throw new HttpResponseException(new HttpResponseMessage() { StatusCode = HttpStatusCode.Unauthorized, Content = new StringContent("Please provide the credentials.") });

            if (IoTUserMrg.IsValidUser(user,false))
            {
                UGToken token = new UGToken(user);
                var status = new LoginResult() { Successeded = true, UGToken = token.Encrypt(), Message = "Successfully signed in." };

                //Get data attach (List<int> storesId) - List store by User
                var lst = new List<int>() { 1 };

                Profile p = new Profile();
                p.Stores = lst;

                string profile = p.SerializeJson();
                //Update profile
                IoTUserMrg.UpdateProfile(user.UserName, profile);

                //status attach data
                status.ProfileJson = profile;
                return status;
            }
            else
            {
                throw new HttpResponseException(new HttpResponseMessage() { StatusCode = HttpStatusCode.Unauthorized, Content = new StringContent("Invalid user name or password.") });
            }
        }
    }
}
