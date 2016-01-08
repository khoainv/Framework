using System;
using System.IO;
using System.Web;

namespace SSD.Web
{
    public class UGLogging
    {
        public static void WriteToLogFile(string logMessage, bool isWebService=true)
        {
            string strLogMessage = string.Empty;
            string strLogFile = "/UGLogging.log";//UG.EC.Common.Common.Constant.LOG_FILE_NAME;
            string AppName = "Application Web";
            strLogFile = HttpContext.Current.Server.MapPath(strLogFile);
            if (isWebService)
            {
                AppName = "Application Webservice";
            }
            StreamWriter swLog;
            string space = "======================================================";

            strLogMessage = string.Format("{0}\r\n {1}\r\n {2}\r\n {3}\r\n", AppName, DateTime.Now, logMessage, space);

            if (!File.Exists(strLogFile))
            {
                swLog = new StreamWriter(strLogFile);
            }
            else
            {
                swLog = File.AppendText(strLogFile);
            }

            swLog.WriteLine(strLogMessage);
            swLog.WriteLine();

            swLog.Close();
        }
    }
}
