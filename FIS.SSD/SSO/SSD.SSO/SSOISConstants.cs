using System.Collections.Generic;
using System.Configuration;
using System.Linq;


namespace SSD.SSO
{
    public class SSOISConstants
    {
        public readonly static string ConnectionStringName = "UGIdentity";

        #region Client
        public readonly static string PathHostIdentityServer = "/core";
        //Not get rootsite on Owin Startup, Rootsite ony after Begin_Request
        public readonly static string LocalClientUri = ConfigurationManager.AppSettings["LocalClientUri"];
        public readonly static List<string> LocalRedirectUris = ConfigurationManager.AppSettings["LocalRedirectUris"].Split(new char[] { ';' }).ToList();
        public readonly static List<string> LocalPostLogoutRedirectUris = ConfigurationManager.AppSettings["LocalPostLogoutRedirectUris"].Split(new char[] { ';' }).ToList();
        public readonly static string LocalClientSecret = ConfigurationManager.AppSettings["LocalClientSecret"];

        public readonly static string LocalClientId = "LocalADIdentityServer";
        #endregion

        public readonly static string SiteName = ConfigurationManager.AppSettings["SiteName"];
        public readonly static bool RequireHTTPS = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["RequireHTTPS"])?true:bool.Parse(ConfigurationManager.AppSettings["RequireHTTPS"]);
    }
}