using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Http;
using System.Threading.Tasks;
using SSD.Framework.Exceptions;
using SSD.Web.Compression;
using SSD.Web.Models;
using SSD.Framework.Security;
using SSD.SampleAPIs.Models;
using SSD.Framework.SSOClient;
using System.Security.Claims;
using SSD.Framework.Models;

namespace SSD.SampleAPIs.Controllers
{
    public class SampleController : SampleBaseApiController
    {
        [DeflateCompression]
        [HttpPost]
        [UGFollowPermission(FollowKey = "SampleController.Get")]
        public async Task<BaseMessage> Get(BaseMessage msg)
        {
            //await Task.Delay(2000);  
            try
            {
                //DecryptData
                msg.MsgJson = DecryptData(msg.MsgJson);
                var para = msg.GetData<IDPara>();
                ValidStoreIDQuery(new List<int>() { para.LocationStoreID });

                var c1 = UGUser.Claims;
                var principal = User as ClaimsPrincipal;
                var obj = from c in principal.Identities.First().Claims
                          select new
                          {
                              c.Type,
                              c.Value
                          };

                return PackageDataWWithSecurity(msg, obj);
            }
            catch (BaseException ex)
            {
                return GetBaseMessage(ex);
            }
            catch (Exception ex)
            {
                return GetBaseMessage(new UnknownException(ex));
            }
        }
    }
}