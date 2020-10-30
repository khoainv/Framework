(function($mvc) {
    if (!$mvc.BingMapSL) {
        var $map = $mvc.BingMapSL = function(divid, options) {
            return new $map.fn.init(divid, options);
        };
        $map.prototype = $map.fn = new $mvc.MapBase;
        $map.fn.init = function(divid, options) {
            var that = this;
            var opts = options;

            var initParams = "";

            initParams = combineInitParams(initParams, "ApplicationId=" + (opts.appid ? opts.appid : "[Bing Maps Key]"));

            if (opts.lat && opts.lng) {
                initParams = combineInitParams(initParams, "Center=" + opts.lat + "_" + opts.lng);
            }

            if (opts.zoom) {
                initParams = combineInitParams(initParams, "ZoomLevel=" + opts.zoom);
            }


            window["MvcMaps_BingMapSL_onSilverlightLoaded_" + opts.id] = function() {
                MvcMaps_BingMapSL_onSilverlightLoaded.apply(that, [opts]);
            };

            $("#" + divid).append("<object id='" + opts.id + "Object' data='data:application/x-silverlight,' type='application/x-silverlight-2' width='100%' height='100%'>" +
            "<param name='source' value='http://dev.virtualearth.net/silverlight/mapcontrol/v1/Microsoft.Maps.MapControl.xap' />" +
            "<param name='onLoad' value='MvcMaps_BingMapSL_onSilverlightLoaded_" + opts.id + "' />" +
            "<param name='enableHtmlAccess' value='true' />" +
            "<param name='initParams' value='" + initParams + "'/>" +
            "</object>");
        };
        $map.fn.plotPushpins = function(pushpins) {
            for (var i = 0; i < pushpins.length; i++) {
                var p = pushpins[i];
                if (p.lat !== undefined && p.lng !== undefined) {
                    this.mapObject.AddChild($CreatePushpin.apply(this, [p]));
                }
            }
        };

        $map.fn.plotPolygons = function(polygons) {
            for (var i = 0; i < polygons.length; i++) {
                var p = polygons[i];
                if (p.lat !== undefined && p.lng !== undefined) {
                    this.mapObject.AddChild($CreatePolygon.apply(this, [p]));
                }
            }
        };

        $map.fn.plotPolylines = function(polylines) {
            for (var i = 0; i < polylines.length; i++) {
                var p = polylines[i];
                if (p.lat !== undefined && p.lng !== undefined) {
                    this.mapObject.AddChild($CreatePolyline.apply(this, [p]));
                }
            }
        };

        $map.fn.clearDynamicMapData = function() {
            this.mapObject.ClearChildren();
        };

        $map.fn.attachDynamicMapLoadEvents = function(options) {
            var that = this;
            var map = this.mapObject;

            map.ViewChangeEnd = function(sender, e) {
                that.loadDynamicMapData();
            };
        };

        $map.fn.getDynamicMapViewData = function() {
            var boundingRect = this.mapObject.BoundingRectangle;
            var l1 = boundingRect.Northeast;
            var l2 = boundingRect.Southwest;
            return {
                minLat: $mvc.Utils.GetLowest(l1.Latitude, l2.Latitude),
                maxLat: $mvc.Utils.GetHighest(l1.Latitude, l2.Latitude),
                minLng: $mvc.Utils.GetLowest(l1.Longitude, l2.Longitude),
                maxLng: $mvc.Utils.GetHighest(l1.Longitude, l2.Longitude)
            };
        };

        $map.fn.init.prototype = $map.fn;

        var $CreatePushpin = function(p) {
            var pushpin = this.slContent.services.createObject("Microsoft.Maps.MapControl.Pushpin");

            pushpin.Location = $CreateLocation.apply(this, [p.lat, p.lng]);

            // ??? this.slContent.xamlLoader.Load("");
            
            return pushpin;
        };

        var $CreatePolygon = function(p) {
            // TODO
            var polygon = this.slContent.services.createObject("Microsoft.Maps.MapControl.MapPolygon");
            for (var pi = 0; pi < p.points.length; pi++) {
                polygon.Locations.Add($CreateLocation.apply(this, [p.points[pi].lat, p.points[pi].lng]));
            }

            //            polygon.Stroke = this.slContent.services.createObject("System.Windows.Media.SolidColorBrush");
            //            polygon.Stroke.Color = this.slContent.services.createObject("System.Windows.Media.Color");
            //            polygon.Stroke.Color = polygon.Stroke.Color.FromArgb(1.0, 255, 0, 0);
            polygon.StrokeThickness = 5;
            return polygon;
        };

        var $CreatePolyline = function(p) {
            // TODO
        };

        var $CreateLocation = function(lat, lng) {
            var loc = this.slContent.services.createObject("Microsoft.Maps.MapControl.Location");
            loc.Latitude = lat;
            loc.Longitude = lng;
            return loc;
        };

        window.MvcMaps_BingMapSL_onSilverlightLoaded = function(options) {
            var that = this;
            var opts = options;

            this.slContent = $("#" + opts.id + "Object")[0].Content;
            this.mapObject = this.slContent.map;

            //this.mapObject.ZoomLevel++;

            var map = this.mapObject;

            if (opts.pushpins) {
                this.plotPushpins(options.pushpins);
            }

            if (opts.polylines) {
                this.plotPolylines(options.polylines);
            }

            if (opts.polygons) {
                this.plotPolygons(options.polygons);
            }

            if (opts.onLoad) {
                opts.onLoad.apply(that, arguments);
            }

            if (opts.dynamicmap) {
                this.setupDynamicMap(opts.dynamicmap);
            }
        };
    }

    function combineInitParams(p, v) {
        var s = p;
        if (s.length > 0) {
            s += ", ";
        }
        s += v;
        return s;
    }
    //    function combineQueryString(querystring, value) {
    //        var str = querystring;
    //        if (str.length > 0) {
    //            str += "&";
    //        }
    //        str += value;
    //        return str;
    //    }
})(MvcMaps);