#region

using System;
using System.ServiceModel;

#endregion

namespace SSD.Framework.Exceptions
{
    public class WebServiceException : FaultException
    {
        public const string MsgError = "Lỗi truy cập webservice. Liên hệ với quản trị viên để trợ giúp.";
        public WebServiceException()
            : base(MsgError, new FaultCode(ErrorCode.WebService.ToString()))
        {
        }
        public WebServiceException(string message)
            : base(message, new FaultCode(ErrorCode.WebService.ToString()))
        {
        }
        public WebServiceException(Exception innerException)
            : base(innerException.ToString(), new FaultCode(ErrorCode.WebService.ToString()))
        {
        }
        public WebServiceException(string message, string action)
            : base(message, new FaultCode(ErrorCode.WebService.ToString()), action)
        {
        }
        public WebServiceException(Exception innerException, string action)
            : base(innerException.ToString(), new FaultCode(ErrorCode.WebService.ToString()), action)
        {
        }
    }
}
