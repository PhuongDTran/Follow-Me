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
        private ShareView _shareView;

		public MainView()
		{
			InitializeComponent();
            _shareView = new ShareView();
            if (Device.RuntimePlatform == Device.Android)
            {
                //MyLocation.IsVisible = false;
            }
            GetCurrentLocation();


        }

        private void OnMyLocationTapped(object sender, EventArgs e)
        {
            GetCurrentLocation();
        }

        private async void ShowPopUp(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(_shareView, true);
        }

        private async void GetCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    MainMap.MoveToRegion( MapSpan.FromCenterAndRadius( new Position( location.Latitude, location.Longitude), Distance.FromMiles(1)));
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                Console.WriteLine("permission exception");
            }
            catch (Exception ex)
            {
                // Unable to get location
                Console.WriteLine("unable to get location");
            }
        }
    }
}
