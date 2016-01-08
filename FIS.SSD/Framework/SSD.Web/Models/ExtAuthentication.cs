using SSD.Framework;
using SSD.Framework.Extensions;

namespace SSD.Web.Models
{
    public partial class ExtAuthentication 
	{
		#region Column Names

        public const string FIELD_ClientId = "ClientId";
        public const string FIELD_ClientSecret = "ClientSecret";
		
		#endregion


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

        public static ExtAuthentication GetObject(string xml)
        {
            return xml.DeserializeXML<ExtAuthentication>();
        }
        public string ToXML()
        {
            return this.SerializeXML<ExtAuthentication>();
        }
     }
}
