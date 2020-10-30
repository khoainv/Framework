using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using SSD.Web.UI.Maps.Utils;

namespace SSD.Web.UI.Maps
{
    public class MapDataResult : ActionResult
    {
        public IEnumerable<Pushpin> Pushpins { get; set; }
        public IEnumerable<Polyline> Polylines { get; set; }
        public IEnumerable<Polygon> Polygons { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var response = context.HttpContext.Response;
            response.ContentType = "application/json";

            var json = new JsonObjectBuilder();

            json.Append("code", 200);

            if (this.Pushpins != null)
            {
                if (this.Pushpins.Count() > 0)
                {
                    var pushpinsJson = new JsonArrayBuilder();
                    foreach (var pin in this.Pushpins)
                    {
                        pushpinsJson.Add(pin);
                    }
                    json.Append("pushpins", pushpinsJson);
                }
            }

            if (this.Polylines != null)
            {
                if (this.Polylines.Count() > 0)
                {
                    var polylinesJson = new JsonArrayBuilder();
                    foreach (var pin in this.Polylines)
                    {
                        polylinesJson.Add(pin);
                    }
                    json.Append("polylines", polylinesJson);
               }
            }

            if (this.Polygons != null)
            {
                if (this.Polygons.Count() > 0)
                {
                    var polygonsJson = new JsonArrayBuilder();
                    foreach (var pin in this.Polygons)
                    {
                        polygonsJson.Add(pin);
                    }
                    json.Append("polygons", polygonsJson);
               }
            }

            response.Write(json.Render());
        }
    }
}
