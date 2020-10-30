using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSD.Web.UI.Maps.Utils;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;

[assembly: System.Web.UI.WebResource("SSD.Web.UI.Maps.BingMapSL.js", "text/javascript")]

namespace SSD.Web.UI.Maps
{
    /// <summary>
    /// This is just the beginnings of a "preview" of this control, and I have found that the
    /// Bing Maps Silverlight controls API for interacting with the Map from JavaScript is a bit limiting.
    /// </summary>
    public class BingMapSL : Map<BingMapSL>
    {
        public BingMapSL(AjaxHelper helper, string mapID)
            : base(helper, mapID, "MvcMaps.BingMapSL")
        {
            this.ScriptInclude("vemap.js", WebUtils.GetWebResourceUrl<BingMapSL>("SSD.Web.UI.Maps.BingMapSL.js"));

            //this.ContainerControl = new HtmlGenericControl("iframe");
        }

        public BingMapSL AppId(string appId)
        {
            this.CustomInitializerOptions.Append("appid", "'" + appId + "'");
            return this;
        }

        protected override object ResolveMapType()
        {
            string strMapType = null;
            switch (this._MapType)
            {
                case MapType.Aerial:
                    strMapType = "'Aerial'";
                    break;
                case MapType.Hybrid:
                    strMapType = "'AerialWithLabels'";
                    break;
                //case MvcMaps.MapType.Terrain:
                //    strMapType = "'Road'";
                //    break;
                default:
                    strMapType = "'Road'";
                    break;
            }
            return strMapType;
        }
    }
}
