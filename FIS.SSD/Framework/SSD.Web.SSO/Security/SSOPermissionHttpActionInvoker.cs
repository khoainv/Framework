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
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Net;
using SSD.Framework.Security;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using SSD.Web.Extensions;
using SSD.Web.SSO;
using SSD.Framework.Models;

namespace SSD.Web.Security
{
    public class SSOPermissionHttpActionInvoker : PermissionHttpActionInvokerBase
    {
        public IoTUserManagerBase IoTUserManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Get<SSOIoTUserManager>();
            }
        }

        protected override HttpResponseMessage CheckPermission(HttpActionContext actionContext, IEnumerable<UGFollowPermissionAttribute> lstAttAjaxAcction)
        {
            var request = actionContext.Request;
            if(HttpContext.Current.User.Identity.IsAuthenticated)
            {
                //string userName = HttpContext.Current.User.GetUserName();
                var headerUsername = request.Headers.GetValues(UGConstants.ClaimTypes.PreferredUserName);
                if (headerUsername != null && headerUsername.Count() > 0)
                {
                    return InternalCheckPermission(headerUsername.First(), IoTUserManager, request, lstAttAjaxAcction);
                }
                else
                {
                    var resultMsgUserName = new BaseMessage("", "", SSD.Framework.Exceptions.ErrorCode.ActionPermission, UGConstants.Security.MsgMissingUserName);
                    HttpResponseMessage replyUserName = request.CreateResponse<BaseMessage>(HttpStatusCode.OK, resultMsgUserName);
                    return replyUserName;
                }
            }
           
            //redirect to Accecc Diney
            var resultMsg = new BaseMessage("", "", SSD.Framework.Exceptions.ErrorCode.ActionPermission, UGConstants.Security.MsgValidLogin);
            HttpResponseMessage reply = request.CreateResponse<BaseMessage>(HttpStatusCode.OK, resultMsg);
            return reply;
        }
    }
}
