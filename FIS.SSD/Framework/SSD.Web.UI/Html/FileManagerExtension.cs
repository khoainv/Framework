using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Globalization;

namespace SSD.Web.UI.Html
{
    public static class FileManagerExtension
    {
        public static MvcHtmlString FileManager(this HtmlHelper helper, string languageCode="vi", bool dock=true)
        {
            var str = new StringBuilder("<div id=\"FileManager\"></div> <script type=\"text/javascript\"> $().ready(function () { ");
            str.Append("var f = $('#FileManager').elfinder({ url: '/FileManager/Files/',")
                .AppendFormat("lang: '{0}',docked: {1}",languageCode,dock?"true":"false")
                .Append(" }) }) </script>");
            return new MvcHtmlString(str.ToString());
        }
    }
}
