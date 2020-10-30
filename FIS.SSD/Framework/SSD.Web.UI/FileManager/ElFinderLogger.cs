using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SSD.Web.UI.FileManager
{
    public class ElFinderLogger : ElFinderILogger
    {

        public void log(string cmd, bool ok, object context, string err, Dictionary<string, object> errorData)
        {
            if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(@"~/log.txt")))
            {
                File.Create(System.Web.HttpContext.Current.Server.MapPath(@"~/log.txt"));
            }

            if (File.GetAttributes(@"~/log.txt") != FileAttributes.ReadOnly)
            {
                string str;
                if (ok)
                {
                    str = "cmd: cmd; OK; context: " + context.ToString().Replace(@"\n", "");
                }
                else
                {
                    str = "cmd: cmd; OK; context: " + context.ToString().Replace(@"\n", ""); 
                }
                File.AppendAllText(@"~/log.txt", str);
            }
        }
    }
}
