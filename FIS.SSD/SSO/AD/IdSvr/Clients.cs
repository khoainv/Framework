using System.Collections.Generic;
using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using System.IO;
using ADIdentityServer.Models;
using System.Xml.Serialization;
using System.Web;

namespace ADIdentityServer.IdSvr
{
    public class Clients
    {
        /// <summary>
        /// Convert array to list
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        private static List<string> ConvertArrayToList(string[] arr)
        {
            List<string> lst = new List<string>();

            if (arr != null && arr.Length > 0)
            {
                foreach (var item in arr)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        lst.Add(item);
                    }
                }
            }
            return lst;
        }

        /// <summary>
        /// Load all config client
        /// </summary>
        /// <returns></returns>
        public static List<Client> Get()
        {
            #region Load the configuration client object
            //FIlename
            string filename = ADISConstants.ClientPath;// HttpContext.Current.Server.MapPath(@"~/App_Configuration/clients.xml");
            // Create a new file stream for reading the XML file
            FileStream ReadFileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            // Create a new XmlSerializer instance with the type of the test class
            XmlSerializer SerializerObj = new XmlSerializer(typeof(ClientConfig));
            // Load the object saved above by using the Deserialize function
            ClientConfig LoadedObj = (ClientConfig)SerializerObj.Deserialize(ReadFileStream);

            // Cleanup
            ReadFileStream.Close();

            if(LoadedObj == null)
            {
                LoadedObj = new ClientConfig();
            }
            List<ClientSetting> lstClientSetting = new List<ClientSetting>()
            {
                new ClientSetting()
                    {
                        Id = ADISConstants.LocalClientId,
                        Name = ADISConstants.SiteName,
                        Secret = ADISConstants.LocalClientSecret,
                        ClientUri = ADISConstants.LocalBaseAddress,
                        RedirectUris = new string[] { ADISConstants.LocalBaseAddress },
                        PostLogoutRedirectUris = new string[] {ADISConstants.LocalBaseAddress },
                        IdentityTokenLifetime = 360,
                        AccessTokenLifetime = 3600
                    }
            };
            if (LoadedObj.ClientSetting == null)
            {
                LoadedObj.ClientSetting = lstClientSetting.ToArray();
            }
            else
            {
                lstClientSetting.AddRange(LoadedObj.ClientSetting);
            }

            //return list
            List<Client> returnList = new List<Client>();

            foreach (var client in lstClientSetting)
            {
                //config
                Client clt = new Client();
                clt.ClientId = client.Id;
                clt.ClientName = client.Name;
                clt.ClientSecrets = new List<Secret>
                    { 
                    new Secret(client.Secret.Sha256())
                    };
                clt.ClientUri = client.ClientUri;
                clt.AccessTokenType = (!string.IsNullOrEmpty(client.AccessTokenType) && client.AccessTokenType.ToLower() == "jwt") ? AccessTokenType.Jwt : AccessTokenType.Reference;
                if (!string.IsNullOrEmpty(client.LogoUri)) { clt.LogoUri = client.LogoUri; }
                clt.RedirectUris = ConvertArrayToList(client.RedirectUris);
                clt.PostLogoutRedirectUris = ConvertArrayToList(client.PostLogoutRedirectUris);
                clt.AllowedCorsOrigins = ConvertArrayToList(client.AllowedCorsOrigins);
                clt.IdentityTokenLifetime = client.IdentityTokenLifetime;
                clt.AccessTokenLifetime = client.AccessTokenLifetime;
                //default
                clt.RequireConsent = false;
                clt.AllowRememberConsent = true;
                clt.Flow = Flows.Implicit;
                clt.AlwaysSendClientClaims = true;
                clt.AllowedScopes = new List<string>
                    {
                    Constants.StandardScopes.OpenId,
                    Constants.StandardScopes.Profile,
                    Constants.StandardScopes.Email,
                    Constants.StandardScopes.Roles,
                    Constants.StandardScopes.OfflineAccess,
                    "read",
                    "write"
                    };
                returnList.Add(clt);
            }
            return returnList;

            #endregion
        }
    }
}