using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;

namespace FollowMeApp.View
{
	public partial class MainView : ContentPage
	{
		public MainView()
		{
			InitializeComponent();
            GetCurrentLocation();
           
		}

        private async void GetCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}");
                    MapSpan span = new MapSpan(new Position(location.Latitude, location.Longitude), 360, 360);
                    Map myMap = new Map(span);
                    myMap.IsShowingUser = true;
                    myMap.MapType = MapType.Street;
                    myMap.HasZoomEnabled = true;
                    MapGrid.Children.Add(myMap);
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
