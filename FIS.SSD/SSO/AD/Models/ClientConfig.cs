using System;
using System.Xml.Serialization;


namespace ADIdentityServer.Models
{
    [Serializable()]
    [System.Xml.Serialization.XmlRoot("ClientConfig")]
    public class ClientConfig
    {
        [XmlArray("ClientSettings")]
        [XmlArrayItem("ClientSetting", typeof(ClientSetting))]
        public ClientSetting[] ClientSetting { get; set; }
    }

    [Serializable()]
    public class ClientSetting
    {
        [System.Xml.Serialization.XmlElement("Id")]
        public string Id { get; set; }

        [System.Xml.Serialization.XmlElement("Name")]
        public string Name { get; set; }

        [System.Xml.Serialization.XmlElement("Secret")]
        public string Secret { get; set; }

        [System.Xml.Serialization.XmlElement("ClientUri")]
        public string ClientUri { get; set; }

        [System.Xml.Serialization.XmlElement("LogoUri")]
        public string LogoUri { get; set; }

        [System.Xml.Serialization.XmlElement("AccessTokenType")]
        public string AccessTokenType { get; set; }

        [XmlArray("RedirectUris")]
        [XmlArrayItem("RedirectUri", typeof(string))]
        public string[] RedirectUris { get; set; }

        [XmlArray("PostLogoutRedirectUris")]
        [XmlArrayItem("PostLogoutRedirectUri", typeof(string))]
        public string[] PostLogoutRedirectUris { get; set; }

        [XmlArray("AllowedCorsOrigins")]
        [XmlArrayItem("AllowedCorsOrigin", typeof(string))]
        public string[] AllowedCorsOrigins { get; set; }

        [System.Xml.Serialization.XmlElement("IdentityTokenLifetime")]
        public int IdentityTokenLifetime { get; set; }

        [System.Xml.Serialization.XmlElement("AccessTokenLifetime")]
        public int AccessTokenLifetime { get; set; }
    }
}