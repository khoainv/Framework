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
using SSD.Framework.Models;

namespace SSD.Web.Security
{
    //http://www.strathweb.com/2014/09/things-didnt-know-action-return-types-asp-net-web-api/
    //Check action permission
    public class PermissionHttpActionInvoker : PermissionHttpActionInvokerBase
    {
        public IoTUserManagerBase IoTUserManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Get<IoTUserManager>();
            }
        }

        protected override HttpResponseMessage CheckPermission(HttpActionContext actionContext, IEnumerable<UGFollowPermissionAttribute> lstAttAjaxAcction)
        {
            var request = actionContext.Request;
            object obj;
            if (request.Properties.TryGetValue(UGConstants.HTTPHeaders.TOKEN_NAME, out obj))
            {
                var token = obj as UGToken;
                if (token != null)
                {
                    string userName = token.UID;
                    return InternalCheckPermission(userName, IoTUserManager, request, lstAttAjaxAcction);
                }
            }
           
            //redirect to Accecc Diney
            var resultMsg = new BaseMessage("", "", SSD.Framework.Exceptions.ErrorCode.ActionPermission, UGConstants.Security.MsgMissingUGToken);
            HttpResponseMessage reply = request.CreateResponse<BaseMessage>(HttpStatusCode.OK, resultMsg);
            return reply;
        }
    }
    public abstract class PermissionHttpActionInvokerBase : IHttpActionInvoker
    {
        protected abstract HttpResponseMessage CheckPermission(HttpActionContext actionContext, IEnumerable<UGFollowPermissionAttribute> lstAttAjaxAcction);
        protected HttpResponseMessage InternalCheckPermission(string userName, IoTUserManagerBase iotMrg, HttpRequestMessage request, IEnumerable<UGFollowPermissionAttribute> lstAttAjaxAcction)
        {
            var user = iotMrg.GetUserCache(userName);
            UGFollowPermissionAttribute att = lstAttAjaxAcction.First() as UGFollowPermissionAttribute;
            if (!user.HasPermission(att.FollowKey))
            {
                string keyName = string.Empty;
                var per = iotMrg.Permissions.Where(x => x.AcctionKey == att.FollowKey);
                if (per.Count() > 0)
                    keyName = per.First().Description;

                //redirect to Accecc Diney
                string error = string.Format(UGConstants.Security.MsgValidAcctionPermission, keyName, att.FollowKey);
                var resultMsg = new BaseMessage("", "", SSD.Framework.Exceptions.ErrorCode.ActionPermission, error);
                HttpResponseMessage reply = request.CreateResponse<BaseMessage>(HttpStatusCode.OK, resultMsg);
                return reply;
            }
            return null;
        }
       
        public Task<HttpResponseMessage> InvokeActionAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            return InvokeActionInternal(actionContext, cancellationToken);
        }
        
        private async Task<HttpResponseMessage> InvokeActionInternal(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (actionContext.ActionDescriptor == null)
            {
                throw new ArgumentNullException("actionContext.ActionDescriptor");
            }

            if (actionContext.ControllerContext == null)
            {
                throw new ArgumentNullException("actionContext.ControllerContext");
            }

            //Check Attr Acction
            var lstAttCtr = actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<UGPermissionAttribute>(false);
            var lstAttAjaxAcction = actionContext.ActionDescriptor.GetCustomAttributes<UGFollowPermissionAttribute>(false);
            if (lstAttAjaxAcction != null && lstAttAjaxAcction.Count > 0)
            {
                var res = CheckPermission(actionContext, lstAttAjaxAcction);
                if (res != null)
                    return res;
            }

            try
            {
                var result = await actionContext.ActionDescriptor.ExecuteAsync(actionContext.ControllerContext, actionContext.ActionArguments, cancellationToken);

                if (result != null)
                    actionContext.Request.Properties["RuntimeReturnType"] = result.GetType();

                var isActionResult = typeof(IHttpActionResult).IsAssignableFrom(actionContext.ActionDescriptor.ReturnType);

                if (result == null && isActionResult)
                {
                    throw new InvalidOperationException();
                }

                if (isActionResult || actionContext.ActionDescriptor.ReturnType == typeof(object))
                {
                    var actionResult = result as IHttpActionResult;

                    if (actionResult == null && isActionResult)
                    {
                        throw new InvalidOperationException();
                    }

                    if (actionResult == null)
                        return actionContext.ActionDescriptor.ResultConverter.Convert(actionContext.ControllerContext, result);

                    var response = await actionResult.ExecuteAsync(cancellationToken);
                    if (response == null)
                    {
                        throw new InvalidOperationException();
                    }

                    if (response.RequestMessage == null)
                    {
                        response.RequestMessage = actionContext.Request;
                    }

                    return response;
                }

                return actionContext.ActionDescriptor.ResultConverter.Convert(actionContext.ControllerContext, result);
            }
            catch (HttpResponseException httpResponseException)
            {
                var response = httpResponseException.Response;
                if (response.RequestMessage == null)
                {
                    response.RequestMessage = actionContext.Request;
                }

                return response;
            }
        }
    }
}
