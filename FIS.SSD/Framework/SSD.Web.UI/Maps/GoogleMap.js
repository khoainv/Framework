(function($mvc) {
    if (!$mvc.GoogleMap) {

        var $map = $mvc.GoogleMap = function(divid, options) {
            return new $map.fn.init(divid, options);
        };
        $map.prototype = $map.fn = new $mvc.MapBase;
        $map.fn.init = function(divid, options) {
            var opts = options;
            var that = this;

            this.id = options.id;
            this.divId = divid;

            var map = this.mapObject = new GMap2($("#" + divid)[0]);

            GEvent.addListener(map, "load", function() {
                if (opts.onLoad) {
                    opts.onLoad.apply(that, arguments);
                }

                if (opts.dynamicmap) {
                    that.setupDynamicMap(opts.dynamicmap);
                }
            });

            map.setCenter(
                (options.lat && options.lng ? new GLatLng(options.lat, options.lng) : null), //new GLatLng(37.4419, -122.1419)
                (options.zoom ? options.zoom : null),
                (options.maptype ? options.maptype : null)
                );

            if (options.fixed === true) {
                map.disableDragging();
            } else {
                map.setUIToDefault();
            }



            if (options.pushpins) {
                this.plotPushpins(options.pushpins);
            }

            if (options.polylines) {
                this.plotPolylines(options.polylines);
            }

            if (options.polygons) {
                this.plotPolygons(options.polygons);
            }
        };

        $map.fn.clearDynamicMapData = function() {
            this.mapObject.clearOverlays();
        };

        $map.fn.plotPushpins = function(pushpins) {
            var len = pushpins.length;
            for (var i = 0; i < len; i++) {
                $plotPushpin.apply(this, [pushpins[i]]);
            }
        };

        $map.fn.plotPolylines = function(polylines) {
            var len = polylines.length;
            for (var i = 0; i < len; i++) {
                $plotPolyline.apply(this, [polylines[i]]);
            }
        };

        $map.fn.plotPolygons = function(polygons) {
            var len = polygons.length;
            for (var i = 0; i < len; i++) {
                $plotPolygon.apply(this, [polygons[i]]);
            }
        };

        function $plotPolyline(polyline) {
            var points = [];
            for (var pi = 0; pi < polyline.points.length; pi++) {
                var p = polyline.points[pi];
                points.push(new GLatLng(p.lat, p.lng));
            }

            var poly = new GPolyline(
                points,
                (polyline.linecolor ? polyline.linecolor : null),
                (polyline.lineweight !== undefined ? polyline.lineweight : null),
                (polyline.lineopacity !== undefined ? polyline.lineopacity : 1.0)
            );

            this.mapObject.addOverlay(poly);
        }

        function $plotPolygon(polygon) {
            var points = [];
            for (var pi = 0; pi < polygon.points.length; pi++) {
                var p = polygon.points[pi];
                points.push(new GLatLng(p.lat, p.lng));
            }

            var poly = new GPolygon(
                points,
                (polygon.linecolor ? polygon.linecolor : null),
                (polygon.lineweight !== undefined ? polygon.lineweight : null),
                (polygon.lineopacity !== undefined ? polygon.lineopacity : 1.0),
                (polygon.fillcolor ? polygon.fillcolor : null),
                (polygon.fillopacity !== undefined ? polygon.fillopacity : 1.0)
            );

            this.mapObject.addOverlay(poly);
        }

        function $plotPushpin(pushpin) {
            var opts = {
                title: (pushpin.title ? pushpin.title : null),
                icon: (pushpin.imageurl ? (function() {
                    var i = new GIcon(G_DEFAULT_ICON);
                    i.image = pushpin.imageurl;

                    i.iconAnchor = new GPoint(0, 0);
                    i.infoWindowAnchor = new GPoint(0, 0);
                    i.shadow = null;

                    var w = 0, h = 0;
                    var xo = 0, yo = 0;

                    if (pushpin.imagesize && pushpin.imagesize.w !== undefined && pushpin.imagesize.h !== undefined) {
                        w = pushpin.imagesize.w;
                        h = pushpin.imagesize.h;
                        i.iconSize = new GSize(w, h);
                    }

                    if (pushpin.G_ImageOffset) {
                        xo = pushpin.G_ImageOffset.x;
                        yo = pushpin.G_ImageOffset.y;
                    } else {
                        xo = (w > 0 ? w / 2 : 0);
                        yo = (h > 0 ? h / 2 : 0);
                    }

                    i.iconAnchor = i.infoWindowAnchor = new GPoint(xo, yo);

                    return i;
                })() : null)
            };

            var marker = new GMarker(new GLatLng(pushpin.lat, pushpin.lng), opts);

            if (pushpin.desc) {
                GEvent.addListener(marker,
                    (pushpin.G_ShowEvent ? pushpin.G_ShowEvent : "click"),
                    function() {
                        marker.openInfoWindowHtml(pushpin.desc);
                    }
                );
            }

            this.mapObject.addOverlay(marker);
        }

        $map.fn.getCenter = function() {
            var center = this.mapObject.getCenter();
            return { lat: center.lat(), lng: center.lng() };
        };
        $map.fn.getZoom = function() {
            return this.mapObject.getZoom();
        };

        //        $map.fn.setCenter = function(v) {
        //            this.mapObject.setCenter(new google.maps.LatLng(v.lat, v.lng));
        //            return this;
        //        };
        //        $map.fn.setZoom = function(v) {
        //            this.mapObject.setZoom(v);
        //            return this;
        //        };

        $map.fn.attachDynamicMapLoadEvents = function(options) {
            var that = this;
            var map = this.mapObject;

            var overlayShown = false;

            GEvent.addListener(map, "click", function(overlay) {
                if (overlay) {
                    overlayShown = true;
                }
            });
            GEvent.addListener(map, "moveend", function() {
                if (overlayShown) {
                    overlayShown = false;
                } else {
                    that.loadDynamicMapData();
                }
            });
        };

        $map.fn.getDynamicMapViewData = function() {
            var bounds = this.mapObject.getBounds();
            var l1 = bounds.getNorthEast();
            var l2 = bounds.getSouthWest();
            return {
                minLat: $mvc.Utils.GetLowest(l1.lat(), l2.lat()),
                maxLat: $mvc.Utils.GetHighest(l1.lat(), l2.lat()),
                minLng: $mvc.Utils.GetLowest(l1.lng(), l2.lng()),
                maxLng: $mvc.Utils.GetHighest(l1.lng(), l2.lng())
            };
        };

        $map.fn.init.prototype = $map.fn;
    }
})(MvcMaps);