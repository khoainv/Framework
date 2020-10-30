using System.Linq;
using System.Data;
using System.Collections.Generic;
using SSD.Framework;
using IdentityServer3.EntityFramework.Entities;
using System.Data.Entity;

namespace SSD.SSO.Identity
{
	public partial class ClientManager : Singleton<ClientManager>
    {
        private ApplicationDbContext _db;
        public ClientManager()
        {
            _db = new ApplicationDbContext();
        }
        public ClientManager(ApplicationDbContext db)
        {
            this._db = db;
        }
        public void Delete(Client client)
        {
            _db.Set<ClientSecret>().RemoveRange(_db.Set<ClientSecret>().Where(x => x.Client.Id == client.Id));
            _db.Set<ClientScope>().RemoveRange(_db.Set<ClientScope>().Where(x => x.Client.Id == client.Id));
            _db.Set<ClientRedirectUri>().RemoveRange(_db.Set<ClientRedirectUri>().Where(x => x.Client.Id == client.Id));
            _db.Set<ClientPostLogoutRedirectUri>().RemoveRange(_db.Set<ClientPostLogoutRedirectUri>().Where(x => x.Client.Id == client.Id));
            _db.Clients.Remove(client);
            _db.SaveChanges();
        }
        public string NewSecret
        {
            get
            {
                return IdentityServer3.Core.Models.HashExtensions.Sha256(System.Guid.NewGuid().ToString()).Substring(0, 25);
            }
        }
        public void Detach(Client client)
        {
            _db.Entry(client).State = EntityState.Detached;
        }
        public void Update(Client client,string ClientSecret,
            string[] selectedScopes,
            string[] RedirectUris,
            string[] PostLogoutRedirectUris)
        {
            //secrect
            string secrect = IdentityServer3.Core.Models.HashExtensions.Sha256(ClientSecret);
            var lstSecrect = _db.Set<ClientSecret>().Where(x => x.Client.Id == client.Id && x.Value == secrect);
            if(lstSecrect.Count()==0)
            {
                _db.Set<ClientSecret>().RemoveRange(_db.Set<ClientSecret>().Where(x => x.Client.Id == client.Id));
                var newSecret = new ClientSecret()
                {
                    Value = secrect,
                    Description = ClientSecret,
                    Type = "SharedSecret",
                    Client = client
                };
                _db.Set<ClientSecret>().Add(newSecret);
            }

            //Scopes
            if (selectedScopes != null && selectedScopes.Length > 0)
            {
                var lstOldScopes = _db.Set<ClientScope>().Where(x => x.Client.Id == client.Id).ToList();
                var lstOldStrScopes = lstOldScopes.Select(x => x.Scope).ToList();

                _db.Set<ClientScope>().RemoveRange(lstOldScopes.Where(x => !selectedScopes.Contains(x.Scope)));

                var lstNewScopes = new List<ClientScope>();
                foreach (var sel in selectedScopes)
                    if (!lstOldStrScopes.Contains(sel))
                        lstNewScopes.Add(new ClientScope() { Scope = sel, Client = client });

                _db.Set<ClientScope>().AddRange(lstNewScopes);
            }
            else
            {
                _db.Set<ClientScope>().RemoveRange(_db.Set<ClientScope>().Where(x => x.Client.Id == client.Id));
            }

            //RedirectUris
            if(RedirectUris==null || RedirectUris.Length == 0)
            {
                if (_db.Set<ClientRedirectUri>().Where(x => x.Client.Id == client.Id).Count() > 0)
                    _db.Set<ClientRedirectUri>().RemoveRange(_db.Set<ClientRedirectUri>().Where(x => x.Client.Id == client.Id));
            }
            else
            {
                var lstOldUri = _db.Set<ClientRedirectUri>().Where(x => x.Client.Id == client.Id).ToList();
                var lstOldStrUri = lstOldUri.Select(x => x.Uri).ToList();

                _db.Set<ClientRedirectUri>().RemoveRange(lstOldUri.Where(x => !RedirectUris.Contains(x.Uri)));

                var lstNewUri = new List<ClientRedirectUri>();
                foreach (var sel in RedirectUris)
                    if (!lstOldStrUri.Contains(sel))
                        lstNewUri.Add(new ClientRedirectUri() { Uri = sel, Client = client });

                _db.Set<ClientRedirectUri>().AddRange(lstNewUri);
            }

            //PostLogoutRedirectUris
            if (PostLogoutRedirectUris == null || PostLogoutRedirectUris.Length == 0)
            {
                if(_db.Set<ClientPostLogoutRedirectUri>().Where(x => x.Client.Id == client.Id).Count()>0)
                    _db.Set<ClientPostLogoutRedirectUri>().RemoveRange(_db.Set<ClientPostLogoutRedirectUri>().Where(x => x.Client.Id == client.Id));
            }
            else
            {
                var lstOldUri = _db.Set<ClientPostLogoutRedirectUri>().Where(x => x.Client.Id == client.Id).ToList();
                var lstOldStrUri = lstOldUri.Select(x => x.Uri).ToList();

                _db.Set<ClientPostLogoutRedirectUri>().RemoveRange(lstOldUri.Where(x => !PostLogoutRedirectUris.Contains(x.Uri)));

                var lstNewUri = new List<ClientPostLogoutRedirectUri>();
                foreach (var sel in PostLogoutRedirectUris)
                    if (!lstOldStrUri.Contains(sel))
                        lstNewUri.Add(new ClientPostLogoutRedirectUri() { Uri = sel, Client = client });

                _db.Set<ClientPostLogoutRedirectUri>().AddRange(lstNewUri);
            }

            
            _db.Entry(client).State = EntityState.Modified;
            _db.SaveChanges();
        }
        public void Create(Client client,
            string ClientSecret,
            string RedirectUri,
            string PostLogoutRedirectUri,
            params string[] selectedScopes)
        {
            client.AllowedScopes = new List<ClientScope>();
            foreach (var scope in selectedScopes)
            {
                client.AllowedScopes.Add(new ClientScope() { Scope = scope });
            }

            client.ClientSecrets = new List<ClientSecret>() {
                    new ClientSecret() { Value = IdentityServer3.Core.Models.HashExtensions.Sha256(ClientSecret),
                        Description =ClientSecret, Type = "SharedSecret" }
                    };

            if (!string.IsNullOrWhiteSpace(RedirectUri))
            {
                client.RedirectUris = new List<ClientRedirectUri>() {
                    new ClientRedirectUri() { Uri = RedirectUri }
                    };
            }
            if (!string.IsNullOrWhiteSpace(PostLogoutRedirectUri))
            {
                client.PostLogoutRedirectUris = new List<ClientPostLogoutRedirectUri>() {
                    new ClientPostLogoutRedirectUri() { Uri = PostLogoutRedirectUri }
                    };
            }

            _db.Clients.Add(client);
            _db.SaveChanges();
        }
        public DbSet<Scope> Scopes
        { get { return _db.Scopes; } }
        public Client FindById(int? id)
        {
            return _db.Clients.Find(id);
        }
        public List<Client> ListClients()
        {
            return _db.Clients.ToList();
        }
        public Client GetClientDefaultImplicit()
        {
            Client client = new Client()
            {
                Flow = IdentityServer3.Core.Models.Flows.Implicit,

                IdentityTokenLifetime = 360,
                AccessTokenLifetime = 3600,
                AuthorizationCodeLifetime = 300,
                AbsoluteRefreshTokenLifetime = 2592000,
                SlidingRefreshTokenLifetime = 1296000,

                RefreshTokenUsage = IdentityServer3.Core.Models.TokenUsage.OneTimeOnly,
                RefreshTokenExpiration = IdentityServer3.Core.Models.TokenExpiration.Absolute,

                AllowRememberConsent = true,
                EnableLocalLogin = true,
                AlwaysSendClientClaims = true,
                PrefixClientClaims = true,
                AllowedScopes = new List<ClientScope>(){ new ClientScope() {Scope = IdentityServer3.Core.Constants.StandardScopes.OpenId },
                    new ClientScope() { Scope = IdentityServer3.Core.Constants.StandardScopes.Email },
                    new ClientScope() { Scope = IdentityServer3.Core.Constants.StandardScopes.Profile },
                    new ClientScope() { Scope = IdentityServer3.Core.Constants.StandardScopes.Roles },
                    }
            };
            return client;
        }
        public Client GetClientDefaultAuthorizationCode()
        {
            Client client = new Client()
            {
                Flow = IdentityServer3.Core.Models.Flows.AuthorizationCode,

                IdentityTokenLifetime = 360,
                AccessTokenLifetime = 3600,
                AuthorizationCodeLifetime = 300,
                AbsoluteRefreshTokenLifetime = 2592000,
                SlidingRefreshTokenLifetime = 1296000,

                AccessTokenType = IdentityServer3.Core.Models.AccessTokenType.Reference,
                RefreshTokenUsage = IdentityServer3.Core.Models.TokenUsage.OneTimeOnly,
                RefreshTokenExpiration = IdentityServer3.Core.Models.TokenExpiration.Absolute,

                RequireConsent = true,
                AllowRememberConsent = true,
                EnableLocalLogin = true,
                AlwaysSendClientClaims = true,
                PrefixClientClaims = true,
                AllowedScopes = new List<ClientScope>(){ new ClientScope() {Scope = IdentityServer3.Core.Constants.StandardScopes.OpenId },
                    new ClientScope() { Scope = IdentityServer3.Core.Constants.StandardScopes.Email },
                    new ClientScope() { Scope = IdentityServer3.Core.Constants.StandardScopes.Profile },
                    new ClientScope() { Scope = IdentityServer3.Core.Constants.StandardScopes.Roles },
                    }
            };
            return client;
        }

        public Client GetClientDefaultHybrid()
        {
            Client client = new Client()
            {
                Flow = IdentityServer3.Core.Models.Flows.Hybrid,

                IdentityTokenLifetime = 360,
                AccessTokenLifetime = 3600,
                AuthorizationCodeLifetime = 300,
                AbsoluteRefreshTokenLifetime = 2592000,
                SlidingRefreshTokenLifetime = 1296000,


                RequireConsent = true,
                AllowRememberConsent = true,
             
                AllowedScopes = new List<ClientScope>(){ new ClientScope() {Scope = IdentityServer3.Core.Constants.StandardScopes.OpenId },
                    new ClientScope() { Scope = IdentityServer3.Core.Constants.StandardScopes.Email },
                    new ClientScope() { Scope = IdentityServer3.Core.Constants.StandardScopes.Profile },
                    new ClientScope() { Scope = IdentityServer3.Core.Constants.StandardScopes.Roles },
                    }
            };
            return client;
        }
        public Client GetClientDefaultKatanaHybrid()
        {
            Client client = new Client()
            {
                Flow = IdentityServer3.Core.Models.Flows.Hybrid,

                IdentityTokenLifetime = 360,
                AccessTokenLifetime = 3600,
                AuthorizationCodeLifetime = 300,
                AbsoluteRefreshTokenLifetime = 2592000,
                SlidingRefreshTokenLifetime = 1296000,


                RequireConsent = false,
                AccessTokenType = IdentityServer3.Core.Models.AccessTokenType.Reference,

                AllowedScopes = new List<ClientScope>(){ new ClientScope() {Scope = IdentityServer3.Core.Constants.StandardScopes.OpenId },
                    new ClientScope() { Scope = IdentityServer3.Core.Constants.StandardScopes.Email },
                    new ClientScope() { Scope = IdentityServer3.Core.Constants.StandardScopes.Profile },
                    new ClientScope() { Scope = IdentityServer3.Core.Constants.StandardScopes.Roles },
                    }
            };
            return client;
        }
        public Client GetClientDefaultClientCredentials()
        {
            Client client = new Client()
            {
                Flow = IdentityServer3.Core.Models.Flows.ClientCredentials,

                IdentityTokenLifetime = 360,
                AccessTokenLifetime = 3600,
                AuthorizationCodeLifetime = 300,
                AbsoluteRefreshTokenLifetime = 2592000,
                SlidingRefreshTokenLifetime = 1296000,

                PrefixClientClaims = false,

                AllowedScopes = new List<ClientScope>(){ new ClientScope() {Scope = IdentityServer3.Core.Constants.StandardScopes.OpenId },
                    new ClientScope() { Scope = IdentityServer3.Core.Constants.StandardScopes.Email },
                    new ClientScope() { Scope = IdentityServer3.Core.Constants.StandardScopes.Profile },
                    new ClientScope() { Scope = IdentityServer3.Core.Constants.StandardScopes.Roles },
                    }
            };
            return client;
        }
        public Client GetClientDefaultCustom()
        {
            Client client = new Client()
            {
                Flow = IdentityServer3.Core.Models.Flows.Custom,

                IdentityTokenLifetime = 360,
                AccessTokenLifetime = 3600,
                AuthorizationCodeLifetime = 300,
                AbsoluteRefreshTokenLifetime = 2592000,
                SlidingRefreshTokenLifetime = 1296000,

                PrefixClientClaims = false,

                AllowedScopes = new List<ClientScope>(){ new ClientScope() {Scope = IdentityServer3.Core.Constants.StandardScopes.OpenId },
                    new ClientScope() { Scope = IdentityServer3.Core.Constants.StandardScopes.Email },
                    new ClientScope() { Scope = IdentityServer3.Core.Constants.StandardScopes.Profile },
                    new ClientScope() { Scope = IdentityServer3.Core.Constants.StandardScopes.Roles },
                    }
            };
            return client;
        }
        public Client GetClientDefaultResourceOwner()
        {
            Client client = new Client()
            {
                Flow = IdentityServer3.Core.Models.Flows.ResourceOwner,

                AccessTokenType = IdentityServer3.Core.Models.AccessTokenType.Jwt,
                AccessTokenLifetime = 3600,
                AbsoluteRefreshTokenLifetime = 86400,
                SlidingRefreshTokenLifetime = 43200,

                IdentityTokenLifetime = 360,
                AuthorizationCodeLifetime = 300,

                RefreshTokenUsage = IdentityServer3.Core.Models.TokenUsage.OneTimeOnly,
                RefreshTokenExpiration = IdentityServer3.Core.Models.TokenExpiration.Sliding,

                AllowedScopes = new List<ClientScope>(){ new ClientScope() {Scope = IdentityServer3.Core.Constants.StandardScopes.OpenId },
                    new ClientScope() { Scope = IdentityServer3.Core.Constants.StandardScopes.Email },
                    new ClientScope() { Scope = IdentityServer3.Core.Constants.StandardScopes.Profile },
                    new ClientScope() { Scope = IdentityServer3.Core.Constants.StandardScopes.Roles },
                    }
            };
            return client;
        }
    }
}
