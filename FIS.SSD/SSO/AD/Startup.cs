/*
 * Copyright 2014 Dominick Baier, Brock Allen
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using ADIdentityServer.IdSvr;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using Owin;
using Serilog;
using Microsoft.Owin;
using ADIdentityServer.Models;
using System.Web.Helpers;
using IdentityServer3.Core;
using IdentityServer3.Core.Extensions;
using System.IdentityModel.Tokens;
using System.Collections.Generic;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using System.Threading.Tasks;
using System.Security.Claims;
using IdentityModel.Client;
using System;
using Microsoft.Owin.Security;
using System.Linq;

[assembly: OwinStartup("ADIdentityServer", typeof(ADIdentityServer.Startup))]
namespace ADIdentityServer
{
    internal class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //string pass = UG.Framework.RSAEngine.Password.EncryptPassword("system!@#$%^");
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.Trace()
               .CreateLogger();

            AntiForgeryConfig.UniqueClaimTypeIdentifier = Constants.ClaimTypes.Subject;
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            //User AD identity
            app.Map(ADISConstants.PathHostIdentityServer, core =>
            {
                var idSvrFactory = Factory.Configure();

                //custom layout
                idSvrFactory.ViewService = new Registration<IViewService>(typeof(CustomViewService));

                //custom token claim
                idSvrFactory.ClaimsProvider = new Registration<IClaimsProvider>(typeof(CustomClaimsProvider));

                //custom AD userservices
                var userService = new ADUserService();
                idSvrFactory.UserService = new Registration<IUserService>(resolver => userService);

                //custom grant
                idSvrFactory.CustomGrantValidators.Add(new Registration<ICustomGrantValidator>(typeof(CustomGrantValidator)));

                var options = new IdentityServerOptions
                {
                    SiteName = ADISConstants.SiteName,
                    RequireSsl = ADISConstants.RequireHTTPS,
                    SigningCertificate = Certificate.Get(),
                    Factory = idSvrFactory,
                    AuthenticationOptions = new IdentityServer3.Core.Configuration.AuthenticationOptions
                    {
                        EnableSignOutPrompt = false,
                        EnablePostSignOutAutoRedirect = true,
                        //PostSignOutAutoRedirectDelay = 0,
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

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = ADISConstants.LocalClientId,
                ClientSecret = ADISConstants.LocalClientSecret,
                Authority = ADISConstants.LocalBaseAddress + ADISConstants.PathHostIdentityServer,
                RedirectUri = ADISConstants.LocalBaseAddress,
                ResponseType = "id_token token",
                Scope = "openid email",
                PostLogoutRedirectUri = ADISConstants.LocalBaseAddress,
                SignInAsAuthenticationType = "Cookies",
                UseTokenLifetime = false,

                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = n =>
                    {
                        // keep the id_token for logout
                        n.AuthenticationTicket.Identity.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));
                        // add access token for sample API
                        n.AuthenticationTicket.Identity.AddClaim(new Claim("access_token", n.ProtocolMessage.AccessToken));
                        // keep track of access token expiration
                        n.AuthenticationTicket.Identity.AddClaim(new Claim("expires_at", DateTimeOffset.Now.AddSeconds(int.Parse(n.ProtocolMessage.ExpiresIn)).ToString()));
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