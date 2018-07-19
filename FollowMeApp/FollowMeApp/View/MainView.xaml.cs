using FollowMeApp.ViewModel;
using Rg.Plugins.Popup.Services;
using System;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FollowMeApp.View
{
    public partial class MainView : ContentPage
	{
        private MainViewModel _mainVM;
        private ShareView _shareView;

		public MainView()
		{
			InitializeComponent();
            _shareView = new ShareView();
            _mainVM = (MainViewModel)BindingContext;
            _mainVM.PropertyChanged += OnUserCurrentPositionChange;
            if (Device.RuntimePlatform == Device.Android)
            {
                MyLocation.IsVisible = false;
            }
        }

        private void OnMyLocationTapped(object sender, EventArgs e)
        {
            
        }

        private async void ShowPopUp(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(_shareView, true);
        }


        private void OnUserCurrentPositionChange(object sender, PropertyChangedEventArgs e)
        {
            var position = _mainVM.UserCurrentPosition;
            if (position != null)
            {
                MainMap.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMiles(1)));
            }
        }


        /*private async void GetCurrentLocation()
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
        }*/
    }
}
