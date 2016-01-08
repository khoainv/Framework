#region

using System;

#endregion

namespace SSD.Framework.Exceptions
{
    public class OpenConnectionSQLException : BaseException
    {
        public const string MsgError = "Lỗi mở kết nối đến hệ quản trị cơ sở dữ liệu. Liên hệ với quản trị viên để trợ giúp.";
        public OpenConnectionSQLException()
            : base(ErrorCode.OpenConnection, MsgError)
        {
        }
        public OpenConnectionSQLException(string message)
            : base(ErrorCode.OpenConnection, message)
        {
        }
        public OpenConnectionSQLException(Exception innerError)
            : base(ErrorCode.OpenConnection, MsgError, innerError)
        {
        }
        public OpenConnectionSQLException(string message, Exception innerError)
            : base(ErrorCode.OpenConnection, message + (innerError == null ? string.Empty : ": " + innerError.Message), innerError)
        {
        }
    }
}
