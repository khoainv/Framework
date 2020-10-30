using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using SSD.Framework;
using SSD.Framework.Exceptions;
using SSD.Framework.Extensions;
using SSD.Web.Api;
using SSD.Web.Models;
using SSD.Web.Security;
using SSD.Web.SSO;
using System.Web;
using Microsoft.AspNet.Identity.Owin;

namespace SSD.SSO.SampleAPIs.Controllers
{
    public class Profile
    {
        public List<int> Stores { get; set; }
    }
    public class SampleBaseApiController : BaseApiController
    {
        public SSOIoTUserManager IoTUserManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Get<SSOIoTUserManager>();
            }
        }

        protected User UGUser { get; private set; }
        public override async Task<HttpResponseMessage> ExecuteAsync(System.Web.Http.Controllers.HttpControllerContext controllerContext, System.Threading.CancellationToken cancellationToken)
        {
            var request = controllerContext.Request;
            var headerUsername = request.Headers.GetValues(UGConstants.ClaimTypes.PreferredUserName);
            if (headerUsername != null && headerUsername.Count() > 0)
            {
                UGUser = IoTUserManager.GetUserCache(headerUsername.First());

                if (UGUser != null && string.IsNullOrWhiteSpace(UGUser.JsonProfile))
                {
                    //Get data attach (List<int> storesId) - List store by User
                    var lst = new List<int>() { 1 };

                    Profile p = new Profile();
                    p.Stores = lst;
                    //Update profile
                    IoTUserManager.UpdateProfile(UGUser.UserName, p.SerializeJson());
                }
            }

            return await base.ExecuteAsync(controllerContext, cancellationToken);
        }
        protected void ValidStoreIDQuery(int storeID)
        {
            ValidStoreIDQuery(new List<int> { storeID });
        }
        protected void ValidStoreIDQuery(List<int> lstStoreID)
        {
            if (UGUser != null && !string.IsNullOrWhiteSpace(UGUser.JsonProfile))
            {
                var pro = UGUser.JsonProfile.DeserializeJson<Profile>();
                foreach (int i in lstStoreID)
                        if (!pro.Stores.Contains(i))
                            throw new BaseException(ErrorCode.Permission, UGConstants.Security.MsgValidStorePermission);
            }
        }
    }
}
