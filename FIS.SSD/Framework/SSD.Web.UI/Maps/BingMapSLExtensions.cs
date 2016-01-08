using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using SSD.Web.UI.Maps.Utils;

namespace SSD.Web.UI.Maps
{
    public static class BingMapSLExtensions
    {
        public static BingMapSL BingMapSL(this AjaxHelper helper)
        {
            return BingMapSL(helper, "B" + GuidUtils.NewShortGuid());
        }

        public static BingMapSL BingMapSL(this AjaxHelper helper, string mapID)
        {
            if (string.IsNullOrEmpty(mapID))
            {
                throw new ArgumentNullException("mapID");
            }
            return new BingMapSL(helper, mapID);
        }
    }
}
