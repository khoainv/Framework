using System.Web.Mvc;
using SSD.Web.UI.Maps.Extensions;
using SSD.Web.UI.Maps.Utils;
using System.Configuration;

// http://msdn.microsoft.com/en-us/library/bb412546.aspx

[assembly: System.Web.UI.WebResource("SSD.Web.UI.Maps.BingMap.js", "text/javascript")]

namespace SSD.Web.UI.Maps
{
    public class BingMap : Map<BingMap>
    {
        public BingMap(AjaxHelper helper, string mapID)
            : base(helper, mapID, "MvcMaps.BingMap")
        {
            this.ScriptInclude("main", "http://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=7.0");
            this.ScriptInclude("bingmap.js", WebUtils.GetWebResourceUrl<BingMap>("SSD.Web.UI.Maps.BingMap.js"));

            this.Key(ConfigurationManager.AppSettings["MvcMaps.Default.BingMap.Key"] ?? "[You Bing Maps Key]");

            this.CssStyle("position", "relative");
        }

        /// <summary>
        /// Sets the Bing Maps API Key to use. You can get one at http://bingmapsportal.com
        /// </summary>
        /// <param name="bingMapsKey">Bing Maps Key</param>
        /// <returns></returns>
        public BingMap Key(string bingMapsKey)
        {
            this.CustomInitializerOptions.Append("B_Key", "'" + bingMapsKey + "'");
            return this;
        }

        protected override object ResolveMapType()
        {
            var prefix = "Microsoft.Maps.MapTypeId.";
            switch (this._MapType)
            {
                case MapType.Aerial:
                    return prefix + "aerial";

                case MapType.Road:
                    return prefix + "road";

                case MapType.Hybrid:
                case MapType.Terrain:
                default:
                    return prefix + "auto";
            }
        }
    }
    /*
    /// <summary>
    /// Renders a Bing Maps map.
    /// </summary>
    public class BingMap : Map<BingMap>
    {
        public BingMap(AjaxHelper helper, string mapID)
            : base(helper, mapID, "MvcMaps.BingMap")
        {
            this.ScriptInclude("main", "http://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6.2");
            this.ScriptInclude("vemap.js", WebUtils.GetWebResourceUrl<BingMap>("MvcMaps.BingMap.js"));

            this.CssStyle("position", "relative");
        }

        /// <summary>
        /// Specifies the VEOrientation (North, South, etc.) to use for Bird's Eye imagery in the Bing Maps map.
        /// </summary>
        /// <param name="birdseyeOrientation">A VEOrientation enumeration value indicating the orientation of the bird's eye map.</param>
        /// <returns></returns>
        public BingMap BirdseyeOrientation(BingOrientation birdseyeOrientation)
        {
            this.CustomInitializerOptions.Append("B_BirdseyeOrientation", birdseyeOrientation.ToJsonValue());
            return this;
        }

        /// <summary>
        /// Specifies the Bing Maps token for the VEMap object. Used to implement Bing Maps Customer Identification.
        /// </summary>
        /// <param name="token">A string representing the Bing Maps token.</param>
        /// <returns></returns>
        public BingMap ClientToken(string token)
        {
            // This is a "VEMap" object specific feature to implement Bing Maps Customer Identification
            this.CustomInitializerOptions.Append("B_ClientToken", string.Format("'{0}'", token));
            return this;
        }

        /// <summary>
        /// Specifies whether or not to enable the Birdseye map style in the Bing Maps map.
        /// </summary>
        /// <param name="enableBirdseye">A boolean value specifying whether or not to enable the Birdseye map style.</param>
        /// <returns></returns>
        public BingMap EnableBirdseye(bool enableBirdseye)
        {
            this.CustomInitializerOptions.Append("B_EnableBirdseye", (enableBirdseye ? "true" : "false"));
            return this;
        }

        /// <summary>
        /// Specifies whether or not labels appear on the Bing Maps map when a user clicks the Aerial or Birdseye map style buttons on the map control dashboard.
        /// </summary>
        /// <param name="enableDashboardLabels">A boolean value specifying whether or not labels appear on the Bing Maps map when a user clicks the Aerial or Birdseye map style buttons on the map control dashboard.</param>
        /// <returns></returns>
        public BingMap EnableDashboardLabels(bool enableDashboardLabels)
        {
            this.CustomInitializerOptions.Append("B_EnableDashboardLabels", (enableDashboardLabels ? "true" : "false"));
            return this;
        }

        //public BingMap GeoRSS(string url)
        //{
        //    this.CustomInitializerOptions.Append("B_GeoRSS", "'" + url + "'");
        //    return this;
        //}

        public BingMap BingCollection(string bingCollectionId)
        {
            this.CustomInitializerOptions.Append("B_BingCollection", "'" + bingCollectionId + "'");
            return this;
        }

        /// <summary>
        /// Specifies whether or not to load the base map tiles.
        /// </summary>
        /// <param name="loadBaseTiles">A boolean value determining whether or not to load the base map tiles.</param>
        /// <returns></returns>
        public BingMap LoadBaseTiles(bool loadBaseTiles)
        {
            this.CustomInitializerOptions.Append("B_LoadBaseTiles", (loadBaseTiles ? "true" : "false"));
            return this;
        }

        /// <summary>
        /// Specifies whether to show the Bing Maps map in 2D or 3D mode.
        /// </summary>
        /// <param name="mapMode">An enumeration specifying the Map Mode to use.</param>
        /// <returns></returns>
        public BingMap MapMode(BingMapMode mapMode)
        {
            this.CustomInitializerOptions.Append("B_Mode", (mapMode == BingMapMode.Mode3D ? "VEMapMode.Mode3D" : "VEMapMode.Mode2D"));
            return this;
        }

        /// <summary>
        /// Specifies whether to show the Map Mode Switch on the Dashboard control of the Bing Maps map.
        /// </summary>
        /// <param name="showSwitch">Defines whether to show the Map Mode Switch on the Dashboard control of the Bing Maps map.</param>
        /// <returns></returns>
        public BingMap MapModeSwitch(bool showSwitch)
        {
            this.CustomInitializerOptions.Append("B_ShowSwitch", (showSwitch ? "true" : "false"));
            return this;
        }

        /// <summary>
        /// Specifies how much tile buffer to use when loading the map. This is ignored in 3D map mode.
        /// </summary>
        /// <param name="tileBuffer">The number of extra boundary tiles to load.</param>
        /// <returns></returns>
        public BingMap TileBuffer(int tileBuffer)
        {
            this.CustomInitializerOptions.Append("B_TileBuffer", tileBuffer.ToString());
            return this;
        }

        protected override object ResolveMapType()
        {
            string strMapType = null;
            switch (this._MapType)
            {
                case MvcMaps.MapType.Aerial:
                    strMapType = "VEMapStyle.Aerial";
                    break;
                case MvcMaps.MapType.Hybrid:
                    strMapType = "VEMapStyle.Hybrid";
                    break;
                case MvcMaps.MapType.Terrain:
                    strMapType = "VEMapStyle.Shaded";
                    break;
            }
            return strMapType;
        }
    }
    */
}

