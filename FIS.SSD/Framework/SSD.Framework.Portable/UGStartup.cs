using System;
using System.Collections.Generic;
using System.Linq;
using SSD.Framework.Helpers;

namespace SSD.Framework
{
    public static class UGStartup
    {
       public static void Register(string urlBase, string iotClientId, string iotSecret)
        {
            Settings.UrlBase = urlBase;
            Settings.IoTClientId = iotClientId;
            Settings.IoTClientSecret = iotSecret;
        }
        public static void Register(string urlBase, string iotClientId, string iotSecret
            , string authorityBase, string clientId, string clientSecret)
        {
            Register(urlBase, iotClientId, iotSecret);
            Settings.AuthorityBase = authorityBase;
            Settings.SSOClientId = clientId;
            Settings.SSOClientSecret = clientSecret;
            Settings.IsUsingSSO = true;
        }
    }
}
