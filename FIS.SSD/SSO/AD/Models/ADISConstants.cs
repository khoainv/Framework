using System.Configuration;
using System.Web;

namespace ADIdentityServer.Models
{
    public class ADISConstants
    {
        #region Client
        public readonly static string PathHostIdentityServer = "/core";
        //Not get rootsite on Owin Startup, Rootsite ony after Begin_Request
        public readonly static string LocalBaseAddress = ConfigurationManager.AppSettings["LocalBaseAddress"];
        public readonly static string LocalClientId = "LocalADIdentityServer";
        public readonly static string LocalClientSecret = "LocalADIdentityServer";

        #endregion
        public readonly static string ClientPath = ConfigurationManager.AppSettings["ClientPath"];
        public readonly static string SiteName = ConfigurationManager.AppSettings["SiteName"];
        public readonly static bool RequireHTTPS = bool.Parse(ConfigurationManager.AppSettings["RequireHTTPS"]);
        public readonly static string DomainNameOrIP = ConfigurationManager.AppSettings["DomainNameOrIP"];
        public readonly static string DomainOU = ConfigurationManager.AppSettings["DomainOU"];
        public readonly static string DomainUser = ConfigurationManager.AppSettings["DomainUser"];
        public readonly static string DomainPassword = UG.Framework.RSAEngine.Password.DecryptPassword(ConfigurationManager.AppSettings["DomainPassword"]);
    }
}