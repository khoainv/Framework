using SSD.Framework.Extensions;

namespace SSD.SSO.Config
{
    public partial class ExtAuthenticationOpenIdConnect
	{
		#region Column Names

        public const string FIELD_ClientId = "ClientId";
        public const string FIELD_Caption = "Caption";
        public const string FIELD_ClientSecret = "ClientSecret";
        public const string FIELD_Authority = "Authority";
        public const string FIELD_RedirectUri = "RedirectUri";
        public const string FIELD_PostLogoutRedirectUri = "PostLogoutRedirectUri";

        #endregion

        public string Authority
        {
            get;
            set;
        }
        public string Caption
        {
            get;
            set;
        }
        public string RedirectUri
        {
            get;
            set;
        }
        public string PostLogoutRedirectUri
        {
            get;
            set;
        }
        public string ClientId
        {
            get;
            set;
        }

        public string ClientSecret
        {
            get;
            set;
        }

        public static ExtAuthenticationOpenIdConnect GetObject(string xml)
        {
            return xml.DeserializeXML<ExtAuthenticationOpenIdConnect>();
        }
        public string ToXML()
        {
            return this.SerializeXML<ExtAuthenticationOpenIdConnect>();
        }
     }
}
