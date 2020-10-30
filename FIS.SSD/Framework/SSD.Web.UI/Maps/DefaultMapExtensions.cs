using System;
using System.Configuration;
using System.Web.Mvc;
using SSD.Web.UI.Maps.Utils;

namespace SSD.Web.UI.Maps
{
    public static class DefaultMapExtensions
    {
        /// <summary>
        /// Retrieves the desired Map Provider that is specified within the applications Web.Config.
        /// </summary>
        /// <param name="helper"></param>
        /// <returns>An instance if IMap to use to render the map.</returns>
        public static IMap DefaultMap(this AjaxHelper helper)
        {
            return DefaultMap(helper, "D" + GuidUtils.NewShortGuid());
        }

        public static IMap DefaultMap(this AjaxHelper helper, string mapID)
        {
            string typeString = ConfigurationManager.AppSettings["MvcMaps.Default.MapProvider"];
            Type type = Type.GetType(typeString);

            var map = (IMap)Activator.CreateInstance(type, helper, mapID);

            string cssClass = ConfigurationManager.AppSettings["MvcMaps.Default.CssClass"];
            if (!string.IsNullOrEmpty(cssClass))
            {
                map.CssClassName = cssClass;
            }

            return map;
        }
    }
}
