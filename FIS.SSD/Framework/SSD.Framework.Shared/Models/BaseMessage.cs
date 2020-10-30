using System;
using Newtonsoft.Json;
using SSD.Framework.Exceptions;

namespace SSD.Framework.Models
{
    public class ErrorCodeJsonConverter : JsonConverter
    {
        #region implemented abstract members of JsonConverter
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var obj = Enum.Parse(typeof(ErrorCode), value.ToString());
            serializer.Serialize(writer, (int)obj);
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Integer)
            {
                var obj = Convert.ToInt32(reader.Value);
                return (ErrorCode)obj;
            }
            else throw new JsonParserException("ErrorCode phải là kiểu int");
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(int) == objectType;//typeof(ErrorCode) == objectType || 
        }

        #endregion

    }

    public class BaseMessage
    {
        public BaseMessage()
        {
        }
        public BaseMessage(string publicKey, string signature, ErrorCode errorCode, string errorMsg)
        {
            PublicKey = publicKey;
            Signature = signature;
            ErrorCode = errorCode;
            ErrorMsg = errorMsg;
        }
        public string MsgJson { get; set; }
        public T GetData<T>()// where T :  new() [T is int, string]
        {
            try
            {
                //T tmp = JsonConvert.DeserializeObject<T>(MsgJson);
                var tmp = (T)JsonConvert.DeserializeObject(MsgJson, typeof(T), new JsonSerializerSettings()
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
                throw new JsonParserException("Lỗi dữ liệu đầu vào hàm. Kiểu dữ liệu PagedPara: " + ex.Message);// (code, msg);  Base Exception header throw new WebServiceException(ex, "Valide Json Object not mask");
            }
        }
        private static void ErrorHandler(object x, Newtonsoft.Json.Serialization.ErrorEventArgs error)
        {
            //throw error.ErrorContext.Error;
            error.ErrorContext.Handled = false;
        }
        //Use fastest JSON serializer https://github.com/kevin-montrose/Jil Not support .Net porable => Error
        public void SetData(object value)
        {
            MsgJson = JsonConvert.SerializeObject(value); // Date time not sync with Jil

            //using (var output = new StringWriter())
            //{
            //    JSON.Serialize(value, output); //not conflic with JsonConvert.SerializeObject(value)
            //    MsgJson = output.ToString();
            //}
        }
        public string PublicKey { get; set; }
        public string Signature { get; set; }
        [JsonConverter(typeof(ErrorCodeJsonConverter))]
        public ErrorCode ErrorCode { get; set; }
        public string ErrorMsg { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        public BaseMessage FromJson(string json)
        {
            return JsonConvert.DeserializeObject(json) as BaseMessage;
        }
    }
}
