#region

using System;

#endregion

namespace SSD.Framework.Exceptions
{
    public class BusinessException : BaseException
    {
        public BusinessException(ErrorCode code, string message)
            : base(code, message)
        {
        }
    }
}
