using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using FollowMeApp.Droid;
using FollowMeApp.Model;
using FollowMeApp.View;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(AndroidCustomMapRenderer))]
namespace FollowMeApp.Droid
{
    public class AndroidCustomMapRenderer : MapRenderer
    {
        IList<Pin> _pins;
        
        public AndroidCustomMapRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                NativeMap.InfoWindowClick -= OnInfoWindowClick;
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                formsMap.PinsCleared += FormsMap_PinsCleared;
                _pins = formsMap.Pins;
                Control.GetMapAsync(this);
            }
        }

        private void FormsMap_PinsCleared(object sender, IList<CirclePin> pins)
        {
            foreach(var pin in pins)
            {
                (pin.Overlay as Circle).Remove();
            }
        }

        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);
            NativeMap.InfoWindowClick += OnInfoWindowClick;
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