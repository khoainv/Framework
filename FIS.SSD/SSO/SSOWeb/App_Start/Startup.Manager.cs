using System;
//using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using Owin;
using SSD.SSO.Identity;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using System.Threading.Tasks;
using SSD.SSO;
using System.Linq;
using System.Security.Claims;
using IdentityServer3.Core;
using IdentityServer3.Core.Extensions;
using SSD.Framework.SSOClient;

namespace SSOWeb
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            //register with IdentityServer3
            //app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
            app.CreatePerOwinContext<ApplicationPermissionManager>(ApplicationPermissionManager.Create);
                
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });
            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = SSOISConstants.LocalClientId,
                ClientSecret = SSOISConstants.LocalClientSecret,
                Authority = SSOISConstants.LocalClientUri + SSOISConstants.PathHostIdentityServer,
                RedirectUri = SSOISConstants.LocalRedirectUris.First(),
                ResponseType = "id_token token",
                Scope = "openid profile email roles",
                PostLogoutRedirectUri = SSOISConstants.LocalPostLogoutRedirectUris.First(),
                SignInAsAuthenticationType = "Cookies",
                UseTokenLifetime = false,
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = async n =>
                    {
                        var nid = new ClaimsIdentity(n.AuthenticationTicket.Identity.Claims,
                            n.AuthenticationTicket.Identity.AuthenticationType,
                            Constants.ClaimTypes.GivenName,
                            Constants.ClaimTypes.Role);

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
                                if (nid.Claims.Where(x => x.Type == ui.Item1 &&x.Value == ui.Item2).Count()<1)
                                    nid.AddClaim(new Claim(ui.Item1, ui.Item2));
                            }
                        }
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

                                //https://identityserver.github.io/Documentation/docsv2/advanced/federated-post-logout-redirect.html
                                //https://github.com/IdentityServer/IdentityServer3/issues/1000
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