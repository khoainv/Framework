using System.Collections.Generic;
using System.Text;

namespace SSD.Web.UI.Maps.Utils
{
    public class JsonObjectBuilder : IJsonRender
    {
        public JsonObjectBuilder()
        {
        }

        public JsonObjectBuilder(JsonObjectBuilder mergeOptions)
        {
            this.Append(mergeOptions);
        }

        protected Dictionary<string, object> _ObjectValues = new Dictionary<string, object>();

        internal Dictionary<string, object> Values
        {
            get
            {
                return this._ObjectValues;
            }
        }

        protected JsonObjectBuilder appendValue(string key, object value)
        {
            this._ObjectValues[key] = value;
            return this;
        }

        public JsonObjectBuilder Append(string key, string value)
        {
            return this.appendValue(key, value);
        }

        public JsonObjectBuilder Append(string key, int value)
        {
            return this.appendValue(key, value);
        }

        public JsonObjectBuilder Append(string key, int? value)
        {
            if (value.HasValue)
            {
                return this.Append(key, value.Value);
            }
            else
            {
                return this;
            }
        }

        public JsonObjectBuilder Append(string key, double value)
        {
            return this.appendValue(key, value);
        }

        public JsonObjectBuilder Append(string key, double? value)
        {
            if (value.HasValue)
            {
                return this.Append(key, value.Value);
            }
            else
            {
                return this;
            }
        }

        public JsonObjectBuilder Append(string key, bool value)
        {
            return this.appendValue(key, value);
        }

        public JsonObjectBuilder Append(string key, bool? value)
        {
            if (value.HasValue)
            {
                return this.Append(key, value.Value);
            }
            else
            {
                return this;
            }
        }

        public JsonObjectBuilder Append(string key, IJsonRender value) // JsonObjectBuilder value)
        {
            return this.appendValue(key, value);
        }

        public JsonObjectBuilder Append(object obj)
        {
            if (obj is JsonObjectBuilder)
            {
                var newValues = ((JsonObjectBuilder)obj).Values;
                foreach(var item in newValues){
                    this.appendValue(item.Key, item.Value);
                }
            }
            else
            {
                var properties = obj.GetType().GetProperties();
                foreach (var prop in properties)
                {
                    this.appendValue(prop.Name, prop.GetValue(obj, null));
                }
            }
            return this;
        }

        public string Render()
        {
            var firstVal = true;

            string strVal;
            var sb = new StringBuilder();
            sb.Append("{");

            foreach (var val in this._ObjectValues)
            {
                if (firstVal)
                {
                    firstVal = false;
                }
                else
                {
                    sb.Append(",");
                }

                //if (val.Value is JsonObjectBuilder)
                //{
                //    strVal = ((JsonObjectBuilder)val.Value).Render();
                //}
                //else if (val.Value is JsonArrayBuilder)
                //{
                //    strVal = ((JsonArrayBuilder)val.Value).Render();
                //}
                if (val.Value is IJsonRender)
                {
                    strVal = ((IJsonRender)val.Value).Render();
                }
                else if (val.Value is bool)
                {
                    strVal = ((bool)val.Value ? "true" : "false");
                }
                else
                {
                    strVal = val.Value.ToString();
                }
                sb.Append(string.Format("{0}:{1}", val.Key, strVal));
            }

            sb.Append("}");

            return sb.ToString();
        }
    }
}
