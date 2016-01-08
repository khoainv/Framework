using SSD.Web.UI.Maps.Utils;

namespace SSD.Web.UI.Maps
{
    public class DynamicMapOptions
    {
        public DynamicMapOptions()
        {
            this.AutoLoad = true;
        }

        public DynamicMapOptions(string url)
            : this()
        {
            this.Url = url;
        }

        public string Url { get; set; }

        /// <summary>
        /// This JavaScript Function that will be executed once data is loaded; everytime data is loaded.
        /// </summary>
        public string DataLoaded { get; set; }

        /// <summary>
        /// This JavaScript Function will be called to display loaded data. This will override the default functionality. Also, if this is set, the DataLoaded property will be ignored.
        /// </summary>
        public string DisplayData { get; set; }

        /// <summary>
        /// A boolean value specifying whether or not the "Dynamic Map" data should be loaded when the Map is loaded. Default is True.
        /// </summary>
        public bool? AutoLoad { get; set; }

        internal void Merge(DynamicMapOptions options)
        {
            if (!string.IsNullOrEmpty(options.Url))
            {
                this.Url = options.Url;
            }
            if (!string.IsNullOrEmpty(options.DataLoaded))
            {
                this.DataLoaded = options.DataLoaded;
            }
            if (!string.IsNullOrEmpty(options.DisplayData))
            {
                this.DisplayData = options.DisplayData;
            }
            if (options.AutoLoad.HasValue)
            {
                this.AutoLoad = options.AutoLoad;
            }
        }

        protected virtual JsonObjectBuilder ToJsonObjectBuilder()
        {
            var json = new JsonObjectBuilder();

            json.Append("url", string.Format("'{0}'", this.Url));

            if (this.AutoLoad.HasValue)
            {
                json.Append("autoload", this.AutoLoad);
            }

            if (!string.IsNullOrEmpty(this.DisplayData))
            {
                json.Append("displaydata", this.DisplayData);
            }
            else
            {
                if (!string.IsNullOrEmpty(this.DataLoaded))
                {
                    json.Append("dataloaded", this.DataLoaded);
                }
            }

            return json;
        }

        public string Render()
        {
            return this.ToJsonObjectBuilder().Render();
        }
    }
}
