using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using FollowMeApp.Droid;
using FollowMeApp.Model;
using FollowMeApp.View;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Android.Util;
using System.Collections.Specialized;
using Xamarin.Forms.Maps.Android;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(CustomMap), typeof(AndroidCustomMapRenderer))]
namespace FollowMeApp.Droid
{
    public class AndroidCustomMapRenderer : MapRenderer
    {
       // private global::Android.Gms.Maps.MapView NativeMap { get { return Control; }}
        private readonly string TAG = "CustomMapRenderer";
        IList<Pin> _pins;
        ObservableCollection<Position> _routeCoordinates;
        private CustomMap _customMap;
        
        public AndroidCustomMapRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName.Equals("VisibleRegion"))
            {
                if (NativeMap == null)
                {
                    Control.GetMapAsync(this);
                }
            }
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                NativeMap.InfoWindowClick -= OnInfoWindowClick;
                _routeCoordinates.CollectionChanged -= _routeCoordinates_CollectionChanged;
                _customMap.PinsCleared -= FormsMap_PinsCleared;
            }

            if (e.NewElement != null)
            {
                _customMap = (CustomMap)e.NewElement;
                _customMap.PinsCleared += _customMap_PinsCleared;
                _pins = _customMap.Pins;
                _routeCoordinates = _customMap.RouteCoordinates;
                
                _routeCoordinates.CollectionChanged += _routeCoordinates_CollectionChanged;
                if(NativeMap == null)
                    Control.GetMapAsync(this);
            }
        }

        private void _customMap_PinsCleared(object sender, IList<CirclePin> pins)
        {
            foreach (var pin in pins)
            {
                if (pin.Overlay != null)
                    (pin.Overlay as Circle).Remove();
            }
        }

        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);
            NativeMap = map;
            NativeMap.InfoWindowClick += OnInfoWindowClick;
            
        }

        private void _routeCoordinates_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //highlighting route
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var latestPosition = _routeCoordinates[_routeCoordinates.Count - 1];
                if(_routeCoordinates.Count > 1)
                {
                    var polylineOptions = new PolylineOptions();
                    polylineOptions.InvokeColor(0x66FF0000);
                    foreach( var position in _routeCoordinates)
                        polylineOptions.Add(new LatLng(position.Latitude, position.Longitude));

                    if (NativeMap == null)
                    {
                        Log.Debug(TAG, "null Native Map");
                    }
                    else
                    {
                        Log.Debug(TAG, "Native Map is not null");
                        Polyline polyline = NativeMap.AddPolyline(polylineOptions);
                    }
                }
                
            }
            else
            {
                Log.Debug(TAG,"not ADD action");
            }
        }

        protected override MarkerOptions CreateMarker(Pin pin)
        {
            if (pin is CirclePin)
            {
                //create an overlay circle, and add to map
                var circleOptions = new CircleOptions();
                circleOptions.InvokeCenter(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
                circleOptions.InvokeRadius(PublishedData.PinOverlayRadius);
                circleOptions.InvokeFillColor(0X66FF0000);
                circleOptions.InvokeStrokeColor(0X66FF0000);
                circleOptions.InvokeStrokeWidth(0);
                Circle circle = NativeMap.AddCircle(circleOptions);
                (pin as CirclePin).Overlay = circle;
            }

            // marker,or pin.
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            marker.Anchor(0.5f, 0.5f);// set anchor to to middle of icon
            marker.SetTitle(pin.Label);
            marker.SetSnippet(pin.Address);
            Bitmap imageBitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.pin);
            Bitmap resizedIcon = Bitmap.CreateScaledBitmap(imageBitmap, 50, 50, false);
            marker.SetIcon(BitmapDescriptorFactory.FromBitmap(resizedIcon));

            return marker;
        }
      
        void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            var customPin = GetCustomPin(e.Marker);
            if (customPin == null)
            {
                throw new Exception("Custom pin not found");
            }
        }

        Pin GetCustomPin(Marker annotation)
        {
            var position = new Position(annotation.Position.Latitude, annotation.Position.Longitude);
            foreach (var pin in _pins)
            {
                if (pin.Position == position)
                {
                    return pin;
                }
            }
            return null;
        }
    }
}