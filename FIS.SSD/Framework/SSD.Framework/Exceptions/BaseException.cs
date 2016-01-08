#region

using System;

#endregion

namespace UG.Framework.Exceptions
{
    public enum ErrorCode
    {
        IsSuccess = 0,
        Permission = 10,
        ActionPermission = 11,
        Signature = 20,
        OpenConnection = 99,
        JsonParser = 100,
        WebService = 101,
        Unknown = 10000,
    }
    public class BaseException : Exception
    {
        #region Fields
        private string mMachine;
        private DateTime mTime;
        private ErrorCode errorNumber;
        private string errorMsg = "";
        #endregion

        #region Constructors
        public BaseException(ErrorCode errorNumber, string errorMsg)
            : base(errorMsg)
        {
            this.mMachine = Environment.MachineName;
            this.mTime = DateTime.Now;
            this.errorNumber = errorNumber;
            this.errorMsg = errorMsg;
        }
        public BaseException(ErrorCode errorNumber, string errorMsg, Exception innerError)
            : base(errorMsg, innerError)
        {
            this.mMachine = Environment.MachineName;
            this.mTime = DateTime.Now;
            this.errorNumber = errorNumber;
            this.errorMsg = errorMsg + (innerError == null ? string.Empty : ": " + innerError.Message);
        }
        #endregion

        #region Properties
        public ErrorCode ErrorNumber
        {
            get { return errorNumber; }
        }
        public string ErrorMsg
        {
            get { return this.errorMsg;}
        }
        public string Machine
        {
            get { return this.mMachine;}
        }
        public DateTime Time
        {
            get { return this.mTime; }
        }
        #endregion
    }
}
