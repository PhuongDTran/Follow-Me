using FollowMeApp.ViewModel;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
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
            _mainVM.PropertyChanged += OnMainViewModelPropertyChanged;
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


        private void OnMainViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_mainVM.MyLocation))
            {
                var position = new Position(_mainVM.MyLocation.Latitude, _mainVM.MyLocation.Longitude);
                MainMap.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMiles(1)));
            }

            if (e.PropertyName == nameof(_mainVM.LeaderLocation))
            {
                var pin = new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(_mainVM.LeaderLocation.Latitude, _mainVM.LeaderLocation.Longitude),
                    Label = "Leader"
                };
                MainMap.Pins.Add(pin);
            }

            if(e.PropertyName == nameof(_mainVM.IsShowingLocation))
            {
                MainMap.IsShowingUser = true;
            }
        }
    }
}
