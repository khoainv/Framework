using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace SSD.Web.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        public static string GetClientIP(this HttpRequestMessage request)
        {
            return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
        }
        public static string GetUserAgent(this HttpRequestMessage request)
        {
            return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserAgent;
        }
        public static Dictionary<string, string> ToDictionary(this string keyValue)
        {
            return keyValue.Split(new[] { "#####" }, StringSplitOptions.RemoveEmptyEntries)
                          .Select(part => part.Split(new[] { "***" }, StringSplitOptions.None))
                          .ToDictionary(split => split[0], split => split[1]);    
        }
    }
}