using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.Default;
using System.Collections.Generic;
using IdentityServer3.Core.Models;
using IdentityServer3.EntityFramework;
using System.Linq;
using SSD.SSO.Identity;
using IdentityServer3.EntityFramework.Entities;
using System.Data.Entity;

namespace SSD.SSO.IdentityServer
{
    public class Factory
    {
        public static IdentityServerServiceFactory Configure(ApplicationDbContext dbContext)
        {
            var efConfig = new EntityFrameworkServiceOptions
            {
                ConnectionString = dbContext.ConnectionStringName,
            };

            // these two calls just pre-populate the test DB from the in-memory config
            //running with db.Clients.Any()
            ConfigureClients(Clients.Get(), efConfig);
            ConfigureScopes(Scopes.Get(), efConfig);


            var factory = new IdentityServerServiceFactory();

            factory.RegisterConfigurationServices(efConfig);
            factory.RegisterOperationalServices(efConfig);

            var viewOptions = new DefaultViewServiceOptions();
            viewOptions.Stylesheets.Add("/Content/Site.css");
            factory.ConfigureDefaultViewService(viewOptions);

            factory.CorsPolicyService = new Registration<ICorsPolicyService>(new DefaultCorsPolicyService { AllowAll = true });

            return factory;
        }
        public static void ConfigureClients(IdentityServer3.Core.Models.Client client, EntityFrameworkServiceOptions options)
        {
            using (var db = new ClientConfigurationDbContext(options.ConnectionString, options.Schema))
            {
                if(db.Clients.Any()&&db.Clients.Where(x=>x.ClientId== SSOISConstants.LocalClientId).Count()>0)
                {
                    //Update Client
                    var dbClient = db.Clients.Where(x => x.ClientId == SSOISConstants.LocalClientId).First();

                    if(db.Clients.Where(x => x.ClientId == SSOISConstants.LocalClientId
                    && x.ClientUri== SSOISConstants.LocalClientUri).Count()==0)
                    {
                        var clt = db.Clients.Where(x => x.ClientId == SSOISConstants.LocalClientId).First();
                        clt.ClientUri = SSOISConstants.LocalClientUri;
                        db.Entry(clt).State = EntityState.Modified;
                    }

                    //Secret
                    string screct = SSOISConstants.LocalClientSecret.Sha256();
                    var lstOldSecret = db.Set<ClientSecret>().Where(x => x.Client.Id == dbClient.Id);
                    if (lstOldSecret.Where(x => x.Value == screct).Count() == 0)
                    {
                        db.Set<ClientSecret>().RemoveRange(lstOldSecret);
                        var lstNewSecret = new List<ClientSecret>() {
                        new ClientSecret() {
                            Value = SSOISConstants.LocalClientSecret.Sha256(),
                            Type = "SharedSecret",
                            Client = dbClient
                        } };
                        db.Set<ClientSecret>().AddRange(lstNewSecret);
                    }

                    //RedirectUris
                    var lstOldRed = db.Set<ClientRedirectUri>().Where(x => x.Client.Id == dbClient.Id);
                    var lstOldStrRed = lstOldRed.Select(x => x.Uri).ToList();
                    db.Set<ClientRedirectUri>().RemoveRange(lstOldRed.Where(x=> !SSOISConstants.LocalRedirectUris.Contains(x.Uri)));
                    var lstNewRed = new List<ClientRedirectUri>();
                    foreach (var sel in SSOISConstants.LocalRedirectUris)
                        if (!lstOldStrRed.Contains(sel))
                            lstNewRed.Add(new ClientRedirectUri() { Uri = sel, Client = dbClient });
                    db.Set<ClientRedirectUri>().AddRange(lstNewRed);

                    //PostLogoutRedirectUris
                    var lstOldPost = db.Set<ClientPostLogoutRedirectUri>().Where(x => x.Client.Id == dbClient.Id);
                    var lstOldStrPost = lstOldRed.Select(x => x.Uri).ToList();
                    db.Set<ClientPostLogoutRedirectUri>().RemoveRange(lstOldPost.Where(x => !SSOISConstants.LocalPostLogoutRedirectUris.Contains(x.Uri)));
                    var lstNewPost = new List<ClientPostLogoutRedirectUri>();
                    foreach (var sel in SSOISConstants.LocalPostLogoutRedirectUris)
                        if (!lstOldStrPost.Contains(sel))
                            lstNewPost.Add(new ClientPostLogoutRedirectUri() { Uri = sel, Client = dbClient });
                    db.Set<ClientPostLogoutRedirectUri>().AddRange(lstNewPost);

                    db.SaveChanges();
                }
                else
                {
                    var e = client.ToEntity();
                    db.Clients.Add(e);
                    db.SaveChanges();
                }
            }
        }
        public static void ConfigureClients(IEnumerable<IdentityServer3.Core.Models.Client> clients, EntityFrameworkServiceOptions options)
        {
            using (var db = new ClientConfigurationDbContext(options.ConnectionString, options.Schema))
            {
                if (!db.Clients.Any())
                {
                    foreach (var c in clients)
                    {
                        var e = c.ToEntity();
                        db.Clients.Add(e);
                    }
                    db.SaveChanges();
                }
            }
        }

        public static void ConfigureScopes(IEnumerable<IdentityServer3.Core.Models.Scope> scopes, EntityFrameworkServiceOptions options)
        {
            using (var db = new ScopeConfigurationDbContext(options.ConnectionString, options.Schema))
            {
                if (!db.Scopes.Any())
                {
                    foreach (var s in scopes)
                    {
                        var e = s.ToEntity();
                        db.Scopes.Add(e);
                    }
                    db.SaveChanges();
                }
            }
        }
    }
}
