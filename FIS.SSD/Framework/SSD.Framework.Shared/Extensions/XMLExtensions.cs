#region

using System;
using Newtonsoft.Json;
using SSD.Framework.Exceptions;

#endregion

namespace SSD.Framework.Extensions
{
    public static class JsonExtensions
    {
        public static T DeserializeJson<T>(this string MsgJson) where T : class, new()
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(MsgJson);
            }
            catch (Exception ex)
            {
                //log error
                throw new JsonParserException("Lỗi dữ liệu Json: " + ex.Message);// (code, msg);  Base Exception header throw new WebServiceException(ex, "Valide Json Object not mask");
            }
        }
        public static string SerializeJson<T>(this T source) where T : class, new()
        {
            try
            {
                return JsonConvert.SerializeObject(source);
            }
            catch (Exception ex)
            {
                //log error
                throw new JsonParserException("Lỗi dữ liệu Json: " + ex.Message);// (code, msg);  Base Exception header throw new WebServiceException(ex, "Valide Json Object not mask");
            }
        }
    }
}
