using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System;
namespace SSD.Web.UI.Maps
{
    public interface IMap
    {
        #region "Properties"

        string MapID { get; }
        HtmlControl ContainerControl { get; set; }
        string CssClassName { get; set; }

        double? CenterLatitude { get; set; }
        double? CenterLongitude { get; set; }
        bool? IsFixed { get; set; }
        int? ZoomLevel { get; set; }
        List<Pushpin> Pushpins { get; set; }
        List<Polyline> Polylines { get; set; }
        List<Polygon> Polygons { get; set; }
        DynamicMapOptions DynamicMapOptions { get; set; }

        #endregion

        void Render();
    }
}
