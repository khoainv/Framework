using SSD.Web.UI.Maps.Extensions;

namespace SSD.Web.UI.Maps
{
    public enum GooglePushpinShowEvents : int
    {
        [JsonValue("'click'")]
        Click = 0,
        [JsonValue("'mouseover'")]
        Mouseover = 1,
        [JsonValue("'dblclick'")]
        DoubleClick = 2
    }
}
