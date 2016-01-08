using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin;
using Owin;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using SSD.Framework;
using SSD.Web.SSO;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using SSD.Framework.SSOClient;
using System;

[assembly: OwinStartupAttribute(typeof(SSD.SSO.SampleAPIs.Startup))]
namespace SSD.SSO.SampleAPIs
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            SSOFactory.CreateOwinContext(app);

            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = UGConstants.SSO.AuthorityBaseUri + UGConstants.SSO.PathHostIdentityServer,
                RequiredScopes = new[] { "openid" },
            });

            //// add app local claims per request
            //app.UseClaimsTransformation(incoming =>
            //{
            //    // either add claims to incoming, or create new principal
            //    var appPrincipal = new ClaimsPrincipal(incoming);
            //    incoming.Identities.First().AddClaim(new Claim("appSpecific", "some_value"));

            //    return Task.FromResult(appPrincipal);
            //});
        }
    }
}
