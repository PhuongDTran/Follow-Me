using FollowMeApp.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.ComponentModel;

namespace FollowMeApp.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        #region Fields
        private readonly INavigationService _navigationService;
        private Location _myLocation;
        private Location _leaderLocation;
        private bool _isShowingLocation = false;
        private IDictionary<string, Location> _members;
        #endregion

        #region Properties

        public bool IsShowingLocation
        {
            get
            {
                return _isShowingLocation;
            }
            private set
            {
                Set(ref _isShowingLocation, value);
            }
        }

        public IDictionary<string, Location> Members
        {
            get
            {
                return _members;
            }
            private set
            {
                Set(ref _members, value);
            }
        }

        public Location MyLocation
        {
            get
            {
                return _myLocation;
            }
            private set
            {
                Set(ref _myLocation, value);
            }
        }

        public Location LeaderLocation
        {
            get
            {
                return _leaderLocation;
            }
            private set
            {
                Set(ref _leaderLocation, value);
            }
        }

        public string StartTripText
        {
            get
            {
                return "Start a trip";
            }
        }

        public string Title
        {
            get
            {
                return "Follow Me";
            }
        }
        #endregion

        #region Constructors
        public MainViewModel() :
           this(null)
        {
            // This no argument constructor is needed for the ViewModelLocator to create an instance of
            //  this view model. The INavigationService is a UI feature that was not copied into this 
            //  project from the original sample code, and so null is passed for the navigation service here.
            // NOTE: in production, we would do this differently, to allow different IDataService and
            //  INavigationService instances to be passed in. This is just for making the basic test work.

        }
        
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        protected MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            //current location updates
            GeolocationManager.instance.LocationUpdatesEvent += OnLocationUpdates;

            //Event handlers when app starts, sleeps, or resumes
            ((App)Application.Current).AppStart += OnForegroundWork;
            ((App)Application.Current).AppSleep += OnBackgroundWok;
            ((App)Application.Current).AppResume += OnForegroundWork;
           
            //listen for any propertis changed in ServerCommunicator
            ServerCommunicator.Instance.PropertyChanged += OnServerCommunicatorPropertyChanged;
        }
        #endregion

        #region Event Subscribers

        private void OnLocationUpdates(object sender, Location location)
        {
                MyLocation = location;
        }

        private async void OnBackgroundWok()
        {
            await GeolocationManager.instance.StopUpdatingLocationAsync();
        }

        private async void OnForegroundWork()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    //if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    //{
                    //    await DisplayAlert("Need location", "Gunna need that location", "OK");
                    //}

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(Permission.Location))
                        status = results[Permission.Location];
                }

                if (status == PermissionStatus.Granted)
                {
                    await GeolocationManager.instance.StartUpdatingLocationAsync();
                    IsShowingLocation = true;
                }
                else if (status != PermissionStatus.Unknown)
                {
                    //await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }

        private async void OnServerCommunicatorPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ServerCommunicator.Instance.GroupId))
            {
                var leaderId = await ServerCommunicator.Instance.SendMemberInfo(_myLocation);
                LeaderLocation = await ServerCommunicator.Instance.GetLocationAsync(leaderId);
            }
        }
        #endregion
    }
}