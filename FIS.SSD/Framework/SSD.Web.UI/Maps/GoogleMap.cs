using System.IO;
using System.Web.Mvc;
using SSD.Web.UI.Maps.Utils;

// http://code.google.com/apis/maps/documentation/
// http://code.google.com/apis/maps/documentation/v3/

[assembly: System.Web.UI.WebResource("SSD.Web.UI.Maps.GoogleMap.js", "text/javascript")]

namespace SSD.Web.UI.Maps
{
    /// <summary>
    /// Renders a Google Maps map.
    /// </summary>
    public class GoogleMap : Map<GoogleMap>
    {
        public GoogleMap(AjaxHelper helper, string mapID)
            : base(helper, mapID, "MvcMaps.GoogleMap")
        {
            //this.ScriptInclude("main", "http://maps.google.com/maps/api/js?sensor=false");
            this.ScriptInclude("main", "http://maps.google.com/maps?file=api&v=2&key=abcdefg&sensor=false");
            this.ScriptInclude("gmap.js", WebUtils.GetWebResourceUrl<BingMap>("SSD.Web.UI.Maps.GoogleMap.js"));

            this.Zoom(4)
                .Center(39.9097362345372, -97.470703125)
                .SetMapType(MapType.Road);
        }

        protected override object ResolveMapType()
        {
            var strMapType = "null";
            switch (this._MapType)
            {
                case MapType.Road:
                    strMapType = "G_NORMAL_MAP";
                    break;
                case MapType.Aerial:
                    strMapType = "G_SATELLITE_MAP";
                    break;
                case MapType.Hybrid:
                    strMapType = "G_HYBRID_MAP";
                    break;
                case MapType.Terrain:
                    strMapType = "G_PHYSICAL_MAP";
                    break;
            }
            return strMapType;
        }
    }
}
