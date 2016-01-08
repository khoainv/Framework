using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using SSD.Web.UI.Maps.Utils;

[assembly: System.Web.UI.WebResource("SSD.Web.UI.Maps.Map.js", "text/javascript")]

namespace SSD.Web.UI.Maps
{
    public abstract class Map<T> : IMap where T: Map<T>
    {
        private string _javascriptObject;

        #region "Constructors"

        public Map(AjaxHelper helper, string mapID, string javascriptObject)
        {
            this._ajaxHelper = helper;
            this.MapID = mapID;
            this._javascriptObject = javascriptObject;
            this.ContainerControl = new HtmlGenericControl("div");
            this.Pushpins = new List<Pushpin>();
            this.Polylines = new List<Polyline>();
            this.Polygons = new List<Polygon>();

            this.ScriptInclude("map.js", WebUtils.GetWebResourceUrl<BingMap>("SSD.Web.UI.Maps.Map.js"));
        }

        #endregion

        #region "Protected Variables"

        protected AjaxHelper _ajaxHelper;
        protected string _mapID;
        protected Dictionary<string, string> _cssStyle = new Dictionary<string, string>();

        protected Dictionary<string, string> _scriptIncludes = new Dictionary<string, string>();

        protected List<object> _loadScripts = new List<object>();

        protected MapType _MapType;

        #endregion

        #region "Private/Protected Properties"

        protected AjaxHelper ajaxHelper
        {
            get { return this._ajaxHelper; }
        }

        protected TextWriter writer
        {
            get
            {
                return this.ajaxHelper.ViewContext.Writer;//.HttpContext.Response.Output;
            }
        }

        #endregion

        #region "Public Properties"

        public string MapID { get; private set; }
        /// <summary>
        /// The client-side "id" attribute of the DIV that will contain the Map.
        /// </summary>
        public string ContainerID
        {
            get
            {
                return "div" + this.MapID;
            }
        }

        public HtmlControl ContainerControl { get; set; }

        public string CssClassName { get; set; }

        private JsonObjectBuilder _CustomInitializerOptions = null;
        protected JsonObjectBuilder CustomInitializerOptions {
            get
            {
                if (this._CustomInitializerOptions == null)
                {
                    this._CustomInitializerOptions = new JsonObjectBuilder();
                }
                return this._CustomInitializerOptions;
            }
        }

        public double? CenterLatitude { get; set; }
        public double? CenterLongitude { get; set; }

        /// <summary>
        /// Determines whether the map can be dragged/zoomed by the user.
        /// </summary>
        public bool? IsFixed { get; set; }

        public int? ZoomLevel { get; set; }

        public List<Pushpin> Pushpins { get; set; }

        public List<Polyline> Polylines { get; set; }

        public List<Polygon> Polygons { get; set; }

        public DynamicMapOptions DynamicMapOptions { get; set; }

        #endregion

        #region "Abstract Methods"

        /// <summary>
        /// This method is used to resolve the set "MapType" to the appropriate client-side json value for the specific mapping provider.
        /// This method is called within the "RenderObjectInitializer" method.
        /// </summary>
        /// <returns></returns>
        protected abstract object ResolveMapType();

        #endregion

        #region "Render Methods"

        public virtual void Render()
        {
            this.ContainerControl.ID = this.ContainerID;

            if (!string.IsNullOrEmpty(this.CssClassName))
            {
                this.ContainerControl.Attributes["class"] = this.CssClassName;
            }

            // Merge in CSS Styles
            foreach(var val in this._cssStyle){
                this.ContainerControl.Style[val.Key] = val.Value;
            }

            using (var tw = new HtmlTextWriter(writer)){
                this.ContainerControl.RenderControl(tw);
            }

            this.RenderScripts(this.writer);
        }

        protected virtual void RenderScripts(TextWriter writer)
        {
            this.RenderScriptIncludes(writer);

            writer.WriteLine("<script type='text/javascript'>");

            this.RenderObjectInitializer(writer);

            this.RenderLoadScripts(writer);

            writer.WriteLine("</script>");
        }

        protected virtual void RenderScriptIncludes(TextWriter writer)
        {
            foreach (var item in this._scriptIncludes)
            {
                if (!string.IsNullOrEmpty(item.Value))
                {
                    writer.WriteLine("<script type='text/javascript' src='{0}'></script>", item.Value);
                }
            }
        }

        protected virtual void RenderObjectInitializer(TextWriter writer)
        {
            writer.Write("var {0} = null;", this.MapID);
            writer.Write("$(function(){");

            writer.Write("{0}=new {1}('{2}',", this.MapID, this._javascriptObject, this.ContainerID);


            var json = new JsonObjectBuilder(this.CustomInitializerOptions);
            json.Append("id", string.Format("'{0}'", this.MapID));

            if (this.CenterLatitude.HasValue && this.CenterLongitude.HasValue)
            {
                json.Append("lat", this.CenterLatitude).Append("lng", this.CenterLongitude);
            }

            if (this.ZoomLevel.HasValue)
            {
                json.Append("zoom", this.ZoomLevel);
            }

            if (this.IsFixed.HasValue)
            {
                json.Append("fixed", (this.IsFixed.Value ? "true" : "false"));
            }

            var resolvedMapType = this.ResolveMapType();
            if (resolvedMapType != null)
            {
                json.Values["maptype"] = resolvedMapType;
            }

            if (this._loadScripts.Count > 0)
            {
                json.Append("onLoad", string.Format("{0}_onLoadMap", this.ContainerID));
            }

            if (this.Pushpins.Count > 0)
            {
                json.Append("pushpins", this.RenderPushpins());
            }

            if (this.Polylines.Count > 0)
            {
                json.Append("polylines", this.RenderPolylines());
            }

            if (this.Polygons.Count > 0)
            {
                json.Append("polygons", this.RenderPolygons());
            }

            if (this.DynamicMapOptions != null)
            {
                json.Append("dynamicmap", this.DynamicMapOptions.Render());
            }

            writer.Write(json.Render());


            writer.Write(");");

            writer.Write("});");
        }

        protected string RenderPushpins()
        {
            var json = new JsonArrayBuilder();
            foreach (var p in this.Pushpins)
            {
                json.Add(p.Render());
            }
            return json.Render();
        }

        protected string RenderPolylines()
        {
            var json = new JsonArrayBuilder();
            foreach (var p in this.Polylines)
            {
                json.Add(p.Render());
            }
            return json.Render();
        }

        protected string RenderPolygons()
        {
            var json = new JsonArrayBuilder();
            foreach (var p in this.Polygons)
            {
                json.Add(p.Render());
            }
            return json.Render();
        }

        protected void RenderLoadScripts(TextWriter writer)
        {
            if (this._loadScripts.Count > 0)
            {
                writer.WriteLine("function {0}_onLoadMap() {{", this.ContainerID);
                foreach (var script in this._loadScripts)
                {
                    if (script is Action)
                    {
                        ((Action)script)();
                    }
                    else
                    {
                        writer.WriteLine(script);
                    }
                }
                writer.WriteLine("}");
            }
        }

        #endregion

        /// <summary>
        /// Adds JavaScript code to execute once the Map has finished loading.
        /// </summary>
        /// <param name="javascript"></param>
        /// <returns></returns>
        public T Load(string javascript)
        {
            this._loadScripts.Add(javascript);
            return this as T;
        }

        /// <summary>
        /// Adds JavaScript code to execute once the Map has finished loading.
        /// </summary>
        /// <param name="javascript"></param>
        /// <returns></returns>
        public T Load(Action javascript)
        {
            this._loadScripts.Add(javascript);
            return this as T;
        }

        /// <summary>
        /// Adds a JavaScript file to be loaded before the map gets loaded/rendered.
        /// </summary>
        /// <param name="javascriptUrl">The URL of the JavaScript file to load.</param>
        /// <returns></returns>
        public T ScriptInclude(string javascriptUrl)
        {
            return this.ScriptInclude(javascriptUrl, javascriptUrl);
        }

        /// <summary>
        /// Adds a JavaScript file to be loaded before the map gets loaded/rendered.
        /// </summary>
        /// <param name="key">A unique key for this script file. If multiple are registered with the same key, only the most recent is used.</param>
        /// <param name="javascriptUrl">The URL of the JavaScript file to load.</param>
        /// <returns></returns>
        public T ScriptInclude(string key, string javascriptUrl)
        {
            this._scriptIncludes[key] = javascriptUrl;
            return this as T;
        }

        /// <summary>
        /// Sets the Latitude/Longitude coordinate to center the map to.
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public T Center(double latitude, double longitude)
        {
            this.CenterLatitude = latitude;
            this.CenterLongitude = longitude;
            return this as T;
        }

        public T AddPushpin(params Pushpin[] pushpins)
        {
            foreach (var p in pushpins)
            {
                this.Pushpins.Add(p);
            }
            return this as T;   
        }

        public T AddPushpin(IEnumerable<Pushpin> pushpins)
        {
            foreach (var p in pushpins)
            {
                this.Pushpins.Add(p);
            }
            return this as T;
        }

        public T AddPolyline(params Polyline[] polylines)
        {
            foreach (var p in polylines)
            {
                this.Polylines.Add(p);
            }
            return this as T;
        }

        public T AddPolyline(IEnumerable<Polyline> polylines)
        {
            foreach (var p in polylines)
            {
                this.Polylines.Add(p);
            }
            return this as T;
        }

        public T AddPolygon(params Polygon[] polygons)
        {
            foreach (var p in polygons)
            {
                this.Polygons.Add(p);
            }
            return this as T;
        }

        public T AddPolygon(IEnumerable<Polygon> polygons)
        {
            foreach (var p in polygons)
            {
                this.Polygons.Add(p);
            }
            return this as T;
        }

        public T DynamicMap(string url)
        {
            string strUrl = url;
            if (strUrl.StartsWith("~"))
            {
                strUrl = VirtualPathUtility.ToAbsolute(strUrl, ajaxHelper.ViewContext.HttpContext.Request.ApplicationPath);
            }
            return this.DynamicMap(new DynamicMapOptions(strUrl));
        }

        //public T DynamicMap(string url, string dataLoaded)
        //{
        //    this.DynamicMap(url);
        //    this.DynamicMapOptions.DataLoaded = dataLoaded;
        //    return this as T;
        //}

        public T DynamicMap(object routeValues)
        {
            var url = new UrlHelper(this.ajaxHelper.ViewContext.RequestContext, this.ajaxHelper.RouteCollection);
            var strUrl = url.RouteUrl(routeValues);
            //return this.DynamicMap(new DynamicMapOptions(strUrl));
            return this.DynamicMap(strUrl);
        }

        public T DynamicMap(object routeValues, DynamicMapOptions options)
        {
            this.DynamicMap(routeValues);
            this.DynamicMapOptions.Merge(options);
            return this as T;
        }

        //public T DynamicMap(object routeValues, string dataLoaded)
        //{
        //    this.DynamicMap(routeValues);
        //    this.DynamicMapOptions.DataLoaded = dataLoaded;
        //    return this as T;
        //}

        public T DynamicMap(DynamicMapOptions options)
        {
            this.DynamicMapOptions = options;
            return this as T;
        }

        /// <summary>
        /// Determines whether the map can be dragged/zoomed by the user.
        /// </summary>
        /// <param name="fixed"></param>
        /// <returns></returns>
        public T Fixed(bool @fixed)
        {
            this.IsFixed = @fixed;
            return this as T;
        }

        /// <summary>
        /// Sets the Map Type/Style to use. This determines which Tile Image Sets are shown on the renderd map.
        /// </summary>
        /// <param name="mapType"></param>
        /// <returns></returns>
        public T SetMapType(MapType mapType)
        {
            this._MapType = mapType;
            return this as T;
        }

        /// <summary>
        /// Sets the Zoom Level of the map.
        /// </summary>
        /// <param name="zoomLevel"></param>
        /// <returns></returns>
        public T Zoom(int zoomLevel)
        {
            this.ZoomLevel = zoomLevel;
            return this as T;
        }

        /// <summary>
        /// Sets the CSS Class for the Map Container HTML Element to use.
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public T CssClass(string className)
        {
            this.CssClassName = className;
            return this as T;
        }

        /// <summary>
        /// Sets/Adds CSS Style attributes to be set on the Map Container HTML Element.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public T CssStyle(string key, string value)
        {
            this._cssStyle[key] = value;
            return this as T;
        }
    }

    

    // Below is an exmaple of how to add an extension method to the Map<T> type.

    //public static class MapExtensions
    //{
    //    public static T Test<T>(this T map)
    //        where T: Map<T>
    //    {
    //        return map;
    //    }
    ////    public static T Center<T>(this T map, double latitude, double longitude)
    ////        where T: Map
    ////    {
    ////        map.CenterLatitude = latitude;
    ////        map.CenterLongitude = longitude;
    ////        return map;
    ////    }

    ////    public static T Zoom<T>(this T map, int zoomLevel)
    ////        where T: Map
    ////    {
    ////        map.ZoomLevel = zoomLevel;
    ////        return map;
    ////    }
    //}
}
