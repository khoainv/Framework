using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Globalization;
using SSD.Web.UI.Maps;

namespace SSD.Web.UI.Html
{
    public static class MapExtension
    {
        public static MvcHtmlString Map(this HtmlHelper helper, IMap map)
        {
            map.Render();
            return new MvcHtmlString("</ br>");
        }
    }
}
