using System;
using Newtonsoft.Json;
using SSD.Framework.Exceptions;

namespace SSD.Framework
{
    public abstract class BaseJsonEntity<T> where T : class, new()
    {
        static object obj = new object();
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (obj)
                    {
                        instance = new T();
                    }
                }
                return instance;
            }
        }
        public T SetData(string jsonMsg)
        {
            try
            {
                var tmp = (T)JsonConvert.DeserializeObject(jsonMsg, typeof(T), new JsonSerializerSettings()
                {
                    MissingMemberHandling = MissingMemberHandling.Error,
                    Error = ErrorHandler
                });
                return tmp;
            }
            catch (Exception ex)
            {
                //TODO
                //log error
                throw new JsonParserException("Lỗi dữ liệu convertTo Entity: " + typeof(T).FullName + " Msg error: " + ex.Message);// (code, msg);  Base Exception header throw new WebServiceException(ex, "Valide Json Object not mask");
            }
        }
        private static void ErrorHandler(object x, Newtonsoft.Json.Serialization.ErrorEventArgs error)
        {
            error.ErrorContext.Handled = false;
        }
        public string GetJsonMsg()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    
}
