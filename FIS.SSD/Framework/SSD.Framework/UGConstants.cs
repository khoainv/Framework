using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Configuration;
using System.Runtime.Serialization.Formatters.Binary;

namespace SSD.Framework
{
    public partial class UGConstants
    {
        public readonly static string UGControllerAssembly = ConfigurationManager.AppSettings["UGControllerAssembly"] ?? string.Empty;

        public readonly static int DefaultPageSize = int.Parse(ConfigurationManager.AppSettings["DefaultPageSize"] ?? "10");
        public readonly static int PageSizeMax = int.Parse(ConfigurationManager.AppSettings["PageSizeMax"] ?? "50");

        public class SSOClient
        {
            public readonly static string ClientId = ConfigurationManager.AppSettings["ClientId"];
            public readonly static string ClientSecret = ConfigurationManager.AppSettings["ClientSecret"];
            public readonly static string ClientUri = ConfigurationManager.AppSettings["ClientUri"];
            public readonly static string RedirectUri = ConfigurationManager.AppSettings["RedirectUri"];
            public readonly static string PostLogoutRedirectUri = ConfigurationManager.AppSettings["PostLogoutRedirectUri"];
        }
        public class SSO
        {
            public readonly static string AuthorityBaseUri = ConfigurationManager.AppSettings["AuthorityBaseUri"];
            public readonly static string PathHostIdentityServer = "/core";
            public readonly static string AuthorizeEndpoint = AuthorityBaseUri + PathHostIdentityServer + "/connect/authorize";
            public readonly static string LogoutEndpoint = AuthorityBaseUri + PathHostIdentityServer + "/connect/endsession";
            public readonly static string TokenEndpoint = AuthorityBaseUri + PathHostIdentityServer + "/connect/token";
            public readonly static string UserInfoEndpoint = AuthorityBaseUri + PathHostIdentityServer + "/connect/userinfo";
            public readonly static string IdentityTokenValidationEndpoint = AuthorityBaseUri + PathHostIdentityServer + "/connect/identitytokenvalidation";
            public readonly static string TokenRevocationEndpoint = AuthorityBaseUri + PathHostIdentityServer + "/connect/revocation";
        }
        public readonly static string GroupAdmin = "Admin";
        public readonly static string GroupAnonymous = "Anonymous";
        public readonly static string AccountAdmin = ConfigurationManager.AppSettings["AccountAdmin"] ?? "admin@7i.com.vn";
        public readonly static string ConnectionStringNameIdentity = ConfigurationManager.AppSettings["ConnectionStringNameIdentity"]??"UGSample";
    }
}
