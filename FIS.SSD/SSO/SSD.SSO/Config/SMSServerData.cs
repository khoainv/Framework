using SSD.Framework.Extensions;

namespace SSD.SSO.Config
{
    public partial class SMSServerData 
	{
		#region Column Names

        public const string FIELD_SID = "SID";
        public const string FIELD_Token = "Token";
        public const string FIELD_FromPhone = "FromPhone";
		
		#endregion


        public string SID
        {
            get;
            set;
        }
        public string Token
        {
            get;
            set;
        }
        public string FromPhone
        {
            get;
            set;
        }

        public static SMSServerData GetObject(string xml)
        {
            return xml.DeserializeXML<SMSServerData>();
        }
        public string ToXML()
        {
            return this.SerializeXML<SMSServerData>();
        }
     }
}
