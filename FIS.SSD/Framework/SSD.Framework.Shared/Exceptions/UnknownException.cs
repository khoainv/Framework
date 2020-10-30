#region

using System;

#endregion

namespace SSD.Framework.Exceptions
{
    public class UnknownException : BaseException
    {
        public const string MsgError = "Lỗi không xác định. Liên hệ với quản trị viên để xử lý.";
        public UnknownException()
            : base(ErrorCode.Unknown, MsgError)
        {
        }
        public UnknownException(string message)
            : base(ErrorCode.Unknown, message)
        {
        }
        public UnknownException(Exception innerError)
            : base(ErrorCode.Unknown, MsgError, innerError)
        {
        }
        public UnknownException(string message, Exception innerError)
            : base(ErrorCode.Unknown, message + (innerError == null ? string.Empty : ": " + innerError.Message), innerError)
        {
        }
    }
}
