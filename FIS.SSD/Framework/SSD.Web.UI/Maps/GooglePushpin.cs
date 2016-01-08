using SSD.Web.UI.Maps.Extensions;
using SSD.Web.UI.Maps.Utils;
using System.Drawing;

namespace SSD.Web.UI.Maps
{
    /// <summary>
    /// A Pushpin object that is specialized for use with the GMap object.
    /// </summary>
    public class GooglePushpin : Pushpin
    {
        public GooglePushpin()
            : base()
        {
            this.ShowInfoEvent = GooglePushpinShowEvents.Click;
        }

        public GooglePushpin(double lat, double lng)
            : this()
        {
            this.Position = new LatLng(lat, lng);
        }

        public GooglePushpin(double lat, double lng, string title)
            : this(lat, lng)
        {
            this.Title = title;
        }

        public GooglePushpin(double lat, double lng, string title, string description)
            : this(lat, lng, title)
        {
            this.Description = description;
        }

        public GooglePushpin(double lat, double lng, string title, string description, string imageUrl)
            : this(lat, lng, title, description)
        {
            this.ImageUrl = imageUrl;
        }

        /// <summary>
        /// An GPushpinShowEvents enumeration that defines which "GMarker" object event will show the Pushpin.Description (Marker InfoWindow Content) to the user.
        /// </summary>
        public GooglePushpinShowEvents ShowInfoEvent { get; set; }

        /// <summary>
        /// A Point representing the image's offset from the icon's anchor.
        /// </summary>
        public Point ImageOffset { get; set; }


        protected override JsonObjectBuilder ToJsonObjectBuilder()
        {
            var json = base.ToJsonObjectBuilder();

            json.Append("G_ShowEvent", this.ShowInfoEvent.ToJsonValue());

            if (this.ImageSize != null)
            {
                var jsonImageSize = new JsonObjectBuilder();
                jsonImageSize.Append("w", this.ImageSize.Width);
                jsonImageSize.Append("h", this.ImageSize.Height);
                json.Append("G_ImageSize", jsonImageSize);
            }

            if (this.ImageOffset != null)
            {
                var jsonImageOffset = new JsonObjectBuilder();
                jsonImageOffset.Append("x", this.ImageOffset.X);
                jsonImageOffset.Append("y", this.ImageOffset.Y);
                json.Append("G_ImageOffset", jsonImageOffset);
            }

            return json;
        }
    }
}