using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using SSD.Framework;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using SSD.Web.Identity;

namespace SSD.Web.Security
{
    public class VerifyClientHandler : VerifyClientHandler<IoTClientManager>
    {
    }
    public class VerifyClientHandler<T> : DelegatingHandler where T : IoTClientManagerBase, new()
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string clientIp = Utility.GetClientIpAddress(request);
            //TODO
            //Check request/ Minute > 1000
            //HttpResponseMessage reply = request.CreateErrorResponse(HttpStatusCode.BadRequest, "HTTP is required for security reason.");
            //return Task.FromResult(reply);
            
            if (request.Headers.Contains(UGConstants.HTTPHeaders.IOT_CLIENT_ID) && request.Headers.Contains(UGConstants.HTTPHeaders.IOT_CLIENT_SECRET))
            {
                string clientId = request.Headers.GetValues(UGConstants.HTTPHeaders.IOT_CLIENT_ID).First();
                string clientSecret = request.Headers.GetValues(UGConstants.HTTPHeaders.IOT_CLIENT_SECRET).First();

                var clientMrg = HttpContext.Current.GetOwinContext().Get<T>();
                var isValid = clientMrg.IsValidClient(clientId, clientSecret);

                if(!isValid)
                {
                    HttpResponseMessage reply = request.CreateErrorResponse(HttpStatusCode.BadRequest, "You not Permission to access APIs, Request Administrator sytem, Please!.");
                    return Task.FromResult(reply);
                }
            }
            else
            {
                HttpResponseMessage reply = request.CreateErrorResponse(HttpStatusCode.BadRequest, "You missing HTTP Headers ClientId or ClientSecret, Request Administrator sytem, Please!.");
                return Task.FromResult(reply);
            }

            return base.SendAsync(request, cancellationToken);
        }        
    }
}