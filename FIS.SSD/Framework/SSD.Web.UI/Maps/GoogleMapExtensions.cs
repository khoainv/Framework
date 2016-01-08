using System;
using System.Web.Mvc;
using SSD.Web.UI.Maps.Utils;

namespace SSD.Web.UI.Maps
{
    public static class GoogleMapExtensions
    {
        public static GoogleMap GoogleMap(this AjaxHelper helper)
        {
            return GoogleMap(helper, "G" + GuidUtils.NewShortGuid());
        }

        public static GoogleMap GoogleMap(this AjaxHelper helper, string mapID)
        {
            if (string.IsNullOrEmpty(mapID))
            {
                throw new ArgumentNullException("mapID");
            }
            var map = new GoogleMap(helper, mapID);
            return map;
        }
    }
}
