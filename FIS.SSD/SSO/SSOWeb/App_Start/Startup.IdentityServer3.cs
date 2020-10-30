using System;
using System.Linq;
using Owin;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security.MicrosoftAccount;
using Microsoft.Owin.Security.Twitter;
using Microsoft.Owin.Security.Facebook;

using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Extensions;
using System.IdentityModel.Tokens;
using System.Collections.Generic;
using Serilog;

using SSD.SSO.IdentityServer;
using SSD.SSO.Identity;
using SSD.SSO.Config;
using SSD.SSO;
using System.Security.Claims;
using IdentityModel.Client;
using Microsoft.IdentityModel.Protocols;
using System.Threading.Tasks;
using IdentityServer3.Core;

namespace SSOWeb
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureIdentityServer3(IAppBuilder app)
        {

            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.Trace()
               .CreateLogger();
            Func<ApplicationDbContext> dbContext = ApplicationDbContext.Create;
            app.CreatePerOwinContext(dbContext);

            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            var db = dbContext.Invoke();
            //Init DB Not Exist => Create => AppConfigBiz running
            db.Database.Initialize(false);
            app.Map("/core", core =>
            {
                var idSvrFactory = Factory.Configure(db);
                idSvrFactory.ConfigureUserService();

                var options = new IdentityServerOptions
                {
                    SiteName = SSOISConstants.SiteName,
                    RequireSsl = SSOISConstants.RequireHTTPS,
                    SigningCertificate = Certificate.Get(),
                    Factory = idSvrFactory,
                    AuthenticationOptions = new AuthenticationOptions
                    {
                        IdentityProviders = ConfigureIdentityProviders,
                        EnableSignOutPrompt = false,
                        EnablePostSignOutAutoRedirect = true,
                    },

                };

                core.UseIdentityServer(options);

                // https://github.com/IdentityServer/Documentation/issues/136
                //https://identityserver.github.io/Documentation/docsv2/advanced/federated-post-logout-redirect.html
                core.Map("/signoutcallback", cleanup =>
                {
                    cleanup.Run(async ctx =>
                    {
                        var state = ctx.Request.Cookies["state"];
                        await ctx.Environment.RenderLoggedOutViewAsync(state);
                    });
                });
                //https://github.com/IdentityServer/IdentityServer3/issues/1000
                core.Map("/post-logout-callback", cb =>
                {
                    cb.Run(async ctx =>
                    {
                        var state = ctx.Request.Cookies["state"];
                        await ctx.Environment.RenderLoggedOutViewAsync(state);
                    });
                });
            });
        }

        public static void ConfigureIdentityProviders(IAppBuilder app, string signInAsType)
        {
            foreach (var authen in AppConfigBiz.Instance.ExtAuthentication)
            {
                //Uncomment the following lines to enable logging in with third party login providers
                if (authen.Key == "MicrosoftAccount")
                    app.UseMicrosoftAccountAuthentication(new MicrosoftAccountAuthenticationOptions()
                    {
                        AuthenticationType = authen.Key,
                        Caption = "Microsoft Account",
                        ClientId = authen.Value.ClientId,
                        ClientSecret = authen.Value.ClientSecret,
                        SignInAsAuthenticationType = signInAsType
                    });

                if (authen.Key == "Twitter")
                    app.UseTwitterAuthentication(new TwitterAuthenticationOptions()
                    {
                        AuthenticationType = authen.Key,
                        Caption = "Twitter",
                        ConsumerKey = authen.Value.ClientId,
                        ConsumerSecret = authen.Value.ClientSecret,
                        SignInAsAuthenticationType = signInAsType
                    });

                if (authen.Key == "Facebook")
                    app.UseFacebookAuthentication(new FacebookAuthenticationOptions()
                    {
                        AuthenticationType = authen.Key,
                        Caption = "Facebook",
                        AppId = authen.Value.ClientId,
                        AppSecret = authen.Value.ClientSecret,
                        SignInAsAuthenticationType = signInAsType
                    });
                if (authen.Key == "Google")
                    app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
                    {
                        AuthenticationType = authen.Key,
                        Caption = "Google",
                        ClientId = authen.Value.ClientId,//ConfigurationManager.AppSettings["GoogleClientId"] == null ? "" : ConfigurationManager.AppSettings["GoogleClientId"].ToString(),
                        ClientSecret = authen.Value.ClientSecret,//ConfigurationManager.AppSettings["GoogleClientSecret"] == null ? "" : ConfigurationManager.AppSettings["GoogleClientSecret"].ToString()
                        SignInAsAuthenticationType = signInAsType,
                    });
            }
            foreach (var authen in AppConfigBiz.Instance.ExtAuthenticationOpenIdConnect)
            {
                app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions()
                {
                    AuthenticationType = authen.Key,
                    Caption = string.IsNullOrWhiteSpace(authen.Value.Caption)? authen.Key: authen.Value.Caption,
                    ClientId = authen.Value.ClientId,
                    ClientSecret = authen.Value.ClientSecret,
                    SignInAsAuthenticationType = signInAsType,
                    Authority = authen.Value.Authority,
                    ResponseType = "id_token",
                    Scope = "openid profile",

                    RedirectUri= authen.Value.RedirectUri,
                    PostLogoutRedirectUri = authen.Value.PostLogoutRedirectUri,

                    Notifications = new OpenIdConnectAuthenticationNotifications
                    {
                        SecurityTokenValidated = n =>
                        {
                            // keep the id_token for logout
                            n.AuthenticationTicket.Identity.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));

                            //n.AuthenticationTicket.Identity.AddClaim(new Claim("access_token", n.ProtocolMessage.AccessToken));
                            //n.AuthenticationTicket.Identity.AddClaim(new Claim("expires_at", DateTimeOffset.Now.AddSeconds(int.Parse(n.ProtocolMessage.ExpiresIn)).ToString()));
                            return Task.FromResult(0);
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
}