using Owin;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using System.Threading.Tasks;
using System.Linq;
using SSD.Framework;
using System.IdentityModel.Tokens;
using System.Collections.Generic;
using SSD.Web.SSO;
using Microsoft.Owin.Security.Cookies;
using System.Security.Claims;
using System;
using SSD.Web.Extensions;
using SSD.Framework.SSOClient;

namespace SSD.SSO.Sample
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            SSOFactory.CreateOwinContext(app);

            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
            //Require => error sso
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });
            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = UGConstants.SSOClient.ClientId,
                ClientSecret = UGConstants.SSOClient.ClientSecret,
                Authority = UGConstants.SSO.AuthorityBaseUri + UGConstants.SSO.PathHostIdentityServer,
                RedirectUri = UGConstants.SSOClient.RedirectUri,
                ResponseType = "id_token token",
                Scope = "openid profile",
                PostLogoutRedirectUri = UGConstants.SSOClient.PostLogoutRedirectUri,
                SignInAsAuthenticationType = "Cookies",
                UseTokenLifetime = false,
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = async n =>
                    {
                        var nid = new ClaimsIdentity(n.AuthenticationTicket.Identity.Claims,
                            n.AuthenticationTicket.Identity.AuthenticationType,
                            UGConstants.ClaimTypes.GivenName,
                            UGConstants.ClaimTypes.Role);
                        // get userinfo data
                        var userInfoClient = new UserInfoClient(
                            new Uri(n.Options.Authority + "/connect/userinfo"),
                            n.ProtocolMessage.AccessToken);
                        var userInfo = await userInfoClient.GetAsync();
                        if (userInfo.Claims != null)
                        {
                            //userInfo.Claims.ToList().ForEach(ui => nid.AddClaim(new Claim(ui.Item1, ui.Item2)));
                            foreach (var ui in userInfo.Claims)
                            {
                                if (nid.Claims.Where(x => x.Type == ui.Item1 && x.Value == ui.Item2).Count() < 1)
                                    nid.AddClaim(new Claim(ui.Item1, ui.Item2));
                            }
                        }
                        string userName = nid.GetUserName();
                        if (string.IsNullOrWhiteSpace(userName))
                            throw new ArgumentNullException("UserName is not null or empty");
                        // keep the id_token for logout
                        nid.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));

                        // add access token for sample API
                        nid.AddClaim(new Claim("access_token", n.ProtocolMessage.AccessToken));

                        // keep track of access token expiration
                        nid.AddClaim(new Claim("expires_at", DateTimeOffset.Now.AddSeconds(int.Parse(n.ProtocolMessage.ExpiresIn)).ToString()));

                        n.AuthenticationTicket = new Microsoft.Owin.Security.AuthenticationTicket(
                            nid,
                            n.AuthenticationTicket.Properties);
                    },
                    RedirectToIdentityProvider = n =>
                    {
                        if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.LogoutRequest)
                        {
                            var idTokenHint = n.OwinContext.Authentication.User.FindFirst("id_token");

                            if (idTokenHint != null)
                            {
                                n.ProtocolMessage.IdTokenHint = idTokenHint.Value;

                                var signOutMessageId = n.OwinContext.Environment.GetSignOutMessageId();
                                if (signOutMessageId != null)
                                {
                                    n.OwinContext.Response.Cookies.Append("state", signOutMessageId);
                                }
                            }
                        }

                        return Task.FromResult(0);
                    }
                }
            });
        }
        
    }
}