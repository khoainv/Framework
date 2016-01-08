using System;
using System.Collections.ObjectModel;
using System.Net;

namespace SSD.Framework.Exceptions
{
    public class HTTPException : Exception
	{
        public HTTPException(HttpStatusCode code, string msg)
            : base(msg)
        {
            HTTPMessage = msg;
            Code = code;
        }
        public HTTPException(HttpStatusCode code, string msg, Exception inner)
            : base(msg, inner)
        {
            HTTPMessage = msg;
            Code = code;
        }
        public HttpStatusCode Code
        {
            get;
            set;
        }
        public string HTTPMessage
        {
            get;
            set;
        }
	}
}

