using System.Collections.Generic;
using SSD.Web.UI.Maps.Utils;

namespace SSD.Web.UI.Maps
{
    public class Polyline : IJsonRender
    {
        public Polyline()
        {
        }

        public Polyline(IEnumerable<LatLng> points)
            : this()
        {
            this.Points = new List<LatLng>(points);
        }

        public List<LatLng> Points { get; set; }

        /// <summary>
        /// Specifies the Line Weight (or Thickness) for the drawn line.
        /// </summary>
        public int? LineWeight { get; set; }

        public string LineColor { get; set; }

        public double? LineOpacity { get; set; }

        protected virtual JsonObjectBuilder ToJsonObjectBuilder(){
            var json = new JsonObjectBuilder();

            var pointsBuilder = new JsonArrayBuilder();
            foreach (var point in this.Points)
            {
                pointsBuilder.Add(point.Render());
            }
            json.Append("points", pointsBuilder.Render());

            if (this.LineWeight.HasValue)
            {
                json.Append("lineweight", this.LineWeight);
            }

            if (!string.IsNullOrEmpty(this.LineColor))
            {
                json.Append("linecolor", "'" + this.LineColor + "'");
            }

            if (this.LineOpacity.HasValue)
            {
                json.Append("lineopacity", this.LineOpacity);
            }

            // Special values that are for Bing Maps, give a better default behavior
            json.Append("B_showicon", false);

            return json;
        }

        public string Render()
        {
            return this.ToJsonObjectBuilder().Render();
        }
    }
}
