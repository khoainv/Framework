using SSD.Framework.Extensions;

namespace SSD.Framework.Email
{
    public partial class SmtpData 
	{
		#region Column Names

        public const string FIELD_Host = "Host";
        public const string FIELD_Port = "Port";
        public const string FIELD_UserName = "UserName";
        public const string FIELD_Password = "Password";
		
		#endregion


        public string Host
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }

        public static SmtpData GetObject(string xml)
        {
            return xml.DeserializeXML<SmtpData>();
        }
        public string ToXML()
        {
            return this.SerializeXML<SmtpData>();
        }
     }
}
