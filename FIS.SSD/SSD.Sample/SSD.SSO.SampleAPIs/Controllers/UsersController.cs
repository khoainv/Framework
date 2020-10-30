using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using SSD.Framework.Extensions;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using SSD.Web.SSO;
using SSD.Framework.Security;
using SSD.Framework.Models;

namespace SSD.SSO.SampleAPIs.Controllers
{
    public class UsersController : ApiController
    {
        private SSOIoTUserManager _iotUserManager;
        public SSOIoTUserManager IoTUserMrg
        {
            get
            {
                if (_iotUserManager == null)
                    _iotUserManager = HttpContext.Current.GetOwinContext().Get<SSOIoTUserManager>();
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

            var accessToken = IoTUserMrg.GetAccessToken(user);
            var userDb = IoTUserMrg.GetUserCache(user.UserName);
            if (!string.IsNullOrWhiteSpace(accessToken) && userDb != null)
            {
                var status = new LoginResult() { Successeded = true, UGToken = accessToken, Message = "Successfully signed in." };

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
        [HttpPost]
        [Authorize]
        public BaseMessage GetProfile(UserAuthen user)
        {
            if (user == null)
                throw new HttpResponseException(new HttpResponseMessage() { StatusCode = HttpStatusCode.Unauthorized, Content = new StringContent("Please provide the credentials.") });

            var userDb = IoTUserMrg.GetUserCache(user.UserName);
            if (userDb != null)
            {
                //Get data attach (List<int> storesId) - List store by User
                var lst = new List<int>() { 1 };

                Profile p = new Profile();
                p.Stores = lst;

                string profile = p.SerializeJson();
                //Update profile
                IoTUserMrg.UpdateProfile(user.UserName, profile);

                BaseMessage msg = new BaseMessage("","",Framework.Exceptions.ErrorCode.IsSuccess,"");
                msg.SetData(profile);
                return msg;
            }
            else
            {
                throw new HttpResponseException(new HttpResponseMessage() { StatusCode = HttpStatusCode.Unauthorized, Content = new StringContent("Invalid user name or password.") });
            }
        }
    }
}
