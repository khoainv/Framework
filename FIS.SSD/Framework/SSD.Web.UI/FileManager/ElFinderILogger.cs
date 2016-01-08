using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSD.Web.UI.FileManager
{
    interface ElFinderILogger
    {
        void log(string cmd, bool ok, object context, string err, Dictionary<string, Object> errorData);
    }
}
