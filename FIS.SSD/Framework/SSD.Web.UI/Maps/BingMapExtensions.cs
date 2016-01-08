using System;
using System.Web.Mvc;
using SSD.Web.UI.Maps.Utils;

namespace SSD.Web.UI.Maps
{
    public static class BingMapExtensions
    {
        public static BingMap BingMap(this AjaxHelper helper)
        {
            return BingMap(helper, "B" + GuidUtils.NewShortGuid());
        }

        public static BingMap BingMap(this AjaxHelper helper, string mapID)
        {
            if (string.IsNullOrEmpty(mapID))
            {
                throw new ArgumentNullException("mapID");
            }
            return new BingMap(helper, mapID);
        }
    }
}
