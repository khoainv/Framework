using SSD.Web.UI.Maps.Extensions;
namespace SSD.Web.UI.Maps
{
    /// <summary>
    /// An enumeration of available directional views for bird's eye imagery on a Bing Maps map.
    /// </summary>
    public enum BingOrientation : int
    {
        /// <summary>
        /// Show image taken looking toward the North.
        /// </summary>
        [JsonValue("VEOrientation.North")]
        North = 0,
        /// <summary>
        /// Show image taken looking toward the South.
        /// </summary>
        [JsonValue("VEOrientation.South")]
        South = 1,
        /// <summary>
        /// Show image taken looking toward the East.
        /// </summary>
        [JsonValue("VEOrientation.East")]
        East = 2,
        /// <summary>
        /// Show image taken looking toward the West.
        /// </summary>
        [JsonValue("VEOrientation.West")]
        West = 3
    }

}
