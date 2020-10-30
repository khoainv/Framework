namespace SSD.Web.UI.Maps
{
    /// <summary>
    /// An enumeration of Map Types (Styles). This is used to determine which Tile Image Sets are shown on the rendered map.
    /// </summary>
    public enum MapType : int
    {
        /// <summary>
        /// Displays the Road map tile imagery.
        /// </summary>
        Road = 0,
        /// <summary>
        /// Displays the Aerial / Satellite map tile imagery.
        /// </summary>
        Aerial = 1,
        /// <summary>
        /// Displays the Aerial map tile imagery with feature (roads, city names, etc.) labels overlaid.
        /// </summary>
        Hybrid = 2,
        /// <summary>
        /// Displays the Shaded / Terrain map tile imagery that displays elevation (mountains, rivers, etc.) contours.
        /// </summary>
        Terrain = 3
    }
}
