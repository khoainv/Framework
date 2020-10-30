using System.Web;
using System.Collections.Generic;
using SSD.Web.UI.Maps.Utils;

namespace SSD.Web.UI.Maps
{
    public class BingPolyline : Polyline, IBingPoly
    {
        public BingPolyline()
        {
        }

        public BingPolyline(IEnumerable<LatLng> points)
            : this()
        {
            this.Points = new List<LatLng>(points);
        }

        #region IVEShape Members

        public bool? ShowIcon { get; set; }

        #endregion

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        protected override JsonObjectBuilder ToJsonObjectBuilder()
        {
            var json = base.ToJsonObjectBuilder();

            if (this.ShowIcon.HasValue)
            {
                json.Append("B_showicon", this.ShowIcon);
            }

            if (!string.IsNullOrEmpty(this.Title))
            {
                json.Append("title", "\"" + this.Title + "\"");
            }

            if (!string.IsNullOrEmpty(this.Description))
            {
                json.Append("desc", "\"" + this.Description + "\"");
            }

            if (!string.IsNullOrEmpty(this.ImageUrl))
            {
                string strImageUrl = this.ImageUrl;
                if (this.ImageUrl.StartsWith("~"))
                {
                    strImageUrl = VirtualPathUtility.ToAbsolute(this.ImageUrl, HttpContext.Current.Request.ApplicationPath);
                }
                json.Append("imageurl", "'" + strImageUrl + "'");
            }

            return json;
        }
    }
}
