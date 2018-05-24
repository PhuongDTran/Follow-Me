using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;

namespace Prototype
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
            {
                Console.WriteLine("on ios");
                
                
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                Console.WriteLine("on android");
            }
        }
        private void Locate_Button(object sender, EventArgs e)
        {
            getCurrentLocation();
        }
        private async void getCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var location = await Geolocation.GetLocationAsync(request);
                if (location != null)
                {
                    var position = new Position( location.Latitude, location.Longitude);
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}");
                    MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMiles(0.5)));
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }
	}
}
