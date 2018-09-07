using FollowMeApp.ViewModel;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using FollowMeApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Device = Xamarin.Forms.Device;

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
                MainMap.IsShowingUser = true;
                MainMap.ClearCirclePins();

                var leaderPosition = new Position(_mainVM.LeaderLocation.Latitude, _mainVM.LeaderLocation.Longitude);
                MainMap.RouteCoordinates.Add(leaderPosition);

                var pin = new CirclePin
                {
                    Type = PinType.Place,
                    Position = leaderPosition,
                    Label = "Leader"
                };
                MainMap.Pins.Add(pin);
                MainMap.MoveToRegion(MapSpan.FromCenterAndRadius(leaderPosition, Distance.FromMiles(1)));
            }

            if (e.PropertyName == nameof(_mainVM.Members))
            {
                MainMap.ClearCirclePins();
                foreach (KeyValuePair<string, Location> entry in _mainVM.Members)
                {
                    if (entry.Value != null)
                    {
                        var pin = new CirclePin
                        {
                            Type = PinType.Place,
                            Position = new Position(entry.Value.Latitude, entry.Value.Longitude),
                            Label = entry.Key
                        };
                        MainMap.Pins.Add(pin);
                    }
                }
            }

            if (e.PropertyName == nameof(_mainVM.IsShowingLocation))
            {
                MainMap.IsShowingUser = true;
            }
        }
    }
}
