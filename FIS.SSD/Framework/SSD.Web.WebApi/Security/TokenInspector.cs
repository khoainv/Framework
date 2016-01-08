using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using SSD.Web.Security;
using System.Net.Http.Headers;
using SSD.Framework;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using SSD.Web.Identity;
using SSD.Framework.Security;

namespace SSD.Web.Security
{
    //check Authorize
    public class TokenInspector : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Contains(UGConstants.HTTPHeaders.TOKEN_NAME))
            {
                string encryptedToken = request.Headers.GetValues(UGConstants.HTTPHeaders.TOKEN_NAME).First();
                try
                {
                    //Giam thieu toi da viec giai ma neu truyen token sai => performance
                    UGToken token = UGToken.Decrypt(encryptedToken);
                    request.Properties.Add(UGConstants.HTTPHeaders.TOKEN_NAME, token);
                    bool requestExpire = new DateTime(token.CRT).AddHours(token.Exp) <= DateTime.Now; //token.ClientIP.Equals(Utility.GetClientIpAddress(request));
                    if (requestExpire)
                    {
                        HttpResponseMessage reply = request.CreateErrorResponse(HttpStatusCode.Created, "Request Expire UGToken.");
                        return Task.FromResult(reply);
                    }
                    var iotMrg = HttpContext.Current.GetOwinContext().Get<IoTUserManager>();
                    UserAuthen user = new UserAuthen(token.UID, token.PWD, "", token.Exp, token.Hash);
                    bool isValidUser = iotMrg.IsValidUser(user);

                    if (!isValidUser)
                    {
                        HttpResponseMessage reply = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid indentity or client machine.");
                        return Task.FromResult(reply);
                    }
                }
                catch
                {
                    HttpResponseMessage reply = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid token.");
                    return Task.FromResult(reply);
                }
            }
            else
            {
                HttpResponseMessage reply = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Request is missing authorization token.");
                return Task.FromResult(reply);
            }
            return base.SendAsync(request, cancellationToken);
        }

    }
}