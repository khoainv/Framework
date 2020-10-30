#region

using System;

#endregion

namespace UG.Framework.Exceptions
{
    public class JsonParserException : BaseException
    {
        public const string MsgError = "Lỗi dữ liệu đầu vào hàm. Json Parser";
        public JsonParserException()
            : base(ErrorCode.JsonParser, MsgError)
        {
        }
        public JsonParserException(string message)
            : base(ErrorCode.JsonParser, message)
        {
        }
        public JsonParserException(Exception innerError)
            : base(ErrorCode.JsonParser, MsgError, innerError)
        {
        }
        public JsonParserException(string message, Exception innerError)
            : base(ErrorCode.JsonParser, message + (innerError == null ? string.Empty : ": " + innerError.Message), innerError)
        {
        }
    }
}
