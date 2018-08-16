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
            _mainVM.PropertyChanged += OnPropertyChange;
            if (Device.RuntimePlatform == Device.Android)
            {
                MyLocation.IsVisible = false;
            }
            
            
        }

        private void OnMyLocationTapped(object sender, EventArgs e)
        {
            
        }

        private async void DisplaySharingMethodsPopup(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(_shareView, true);
        }


        private void OnPropertyChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_mainVM.UserCurrentPosition))
            {
                MainMap.MoveToRegion(MapSpan.FromCenterAndRadius(_mainVM.UserCurrentPosition, Distance.FromMiles(1)));
            }

            if (e.PropertyName == nameof(_mainVM.LeaderLocation))
            {
                Pin pin = new Pin
                {
                    Position = new Position( _mainVM.LeaderLocation.Latitude, _mainVM.LeaderLocation.Longitude),
                    Label = "neighbors"
                };
                MainMap.Pins.Add(pin);
            }
        }
    }
}
