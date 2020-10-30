using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSD.Web.UI.Maps.Utils;

namespace SSD.Web.UI.Maps
{
    public class LatLng
    {
        public LatLng(double lat, double lng)
        {
            this.Latitude = lat;
            this.Longitude = lng;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        internal virtual JsonObjectBuilder ToJsonObjectBuilder()
        {
            var json = new JsonObjectBuilder();
            json.Append("lat", this.Latitude);
            json.Append("lng", this.Longitude);
            return json;
        }

        public string Render()
        {
            return this.ToJsonObjectBuilder().Render();
        }
    }
}
