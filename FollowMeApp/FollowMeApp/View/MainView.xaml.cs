using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using FollowMeApp.ViewModel;
using Rg.Plugins.Popup;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;

namespace FollowMeApp.View
{
	public partial class MainView : ContentPage
	{
        private StartViewModel viewModel;
        private SharingView _sharingView;

		public MainView()
		{
			InitializeComponent();
            viewModel = (StartViewModel)BindingContext;
            _sharingView = new SharingView();
            if (true)
            {
                //Position currentPosition = viewModel.CurrentPosition;
                //MapSpan span = new MapSpan(currentPosition, 360, 360);
                //AppMap.MoveToRegion(span);
                //GetCurrentLocation();
            }


        }

        private async void ShowPopUp(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(_sharingView, true);
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
