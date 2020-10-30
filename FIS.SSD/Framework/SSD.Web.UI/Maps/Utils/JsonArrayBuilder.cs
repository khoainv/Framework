using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSD.Web.UI.Maps.Utils
{
    public class JsonArrayBuilder : IJsonRender
    {
        private List<Object> _jsonObjects;

        public JsonArrayBuilder()
        {
            this._jsonObjects = new List<object>();
        }

        //public void Add(JsonObjectBuilder obj)
        //{
        //    this._jsonObjects.Add(obj);
        //}

        public void Add(IJsonRender obj)
        {
            this._jsonObjects.Add(obj);
        }

        public void Add(string obj)
        {
            this._jsonObjects.Add(obj);
        }

        public string Render()
        {
            var sb = new StringBuilder("[");

            var firstVal = true;
            foreach (var val in this._jsonObjects)
            {
                if (firstVal)
                {
                    firstVal = false;
                }
                else
                {
                    sb.Append(",");
                }

                //if (val is JsonObjectBuilder)
                //{
                //    sb.Append(((JsonObjectBuilder)val).Render());
                //}
                //else if (val is JsonArrayBuilder)
                //{
                //    sb.Append(((JsonArrayBuilder)val).Render());
                //}
                if (val is IJsonRender)
                {
                    sb.Append(((IJsonRender)val).Render());
                }
                else
                {
                    sb.Append(val.ToString());
                }
            }

            sb.Append("]");
            return sb.ToString();
        }
    }
}
