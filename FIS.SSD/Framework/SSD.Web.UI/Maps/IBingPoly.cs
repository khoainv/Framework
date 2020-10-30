namespace SSD.Web.UI.Maps
{
    /// <summary>
    /// This interface is used to specify specific properties of the Bing Maps Polygon and Polyline objects (BingPolygon and BingPolyline)
    /// </summary>
    public interface IBingPoly
    {
        bool? ShowIcon { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        string ImageUrl { get; set; }
    }
}
