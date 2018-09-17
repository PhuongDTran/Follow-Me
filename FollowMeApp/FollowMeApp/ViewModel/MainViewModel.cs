using FollowMeApp.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using GalaSoft.MvvmLight.Messaging;
using System.ComponentModel;
using System.Threading.Tasks;
using Device = Xamarin.Forms.Device;

namespace FollowMeApp.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        #region Fields
        private readonly INavigationService _navigationService;
        private Location _myLocation;
        private Location _leaderLocation;
        private bool _isShowingLocation = false;
        private bool _hasSent = false;
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

        public IDictionary<string, Location> Members { get; set; }

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
            ((App)Application.Current).AppSleep += OnBackgroundWork;
            ((App)Application.Current).AppResume += OnForegroundWork;

            Messenger.Default.Register<string>(this, PublishedData.MemberLocationNotification, async (memberId) =>
              {
                  Location location = await ServerCommunicator.Instance.GetLocationAsync(memberId);
                  Device.BeginInvokeOnMainThread(() =>
                  {
                      if (Members == null)
                          Members = new Dictionary<string, Location>();

                      if (Members.Keys.Contains(memberId))
                          Members[memberId] = location;
                      else
                          Members.Add(memberId, location);

                      RaisePropertyChanged("Members");
                  });
              });

            Messenger.Default.Register<string>(this, PublishedData.GroupIdNotification, async (leaderId) =>
            {
                if (leaderId != null)
                {
                    var location = await ServerCommunicator.Instance.GetLocationAsync(leaderId);
                    Device.BeginInvokeOnMainThread(() =>
                   {
                       LeaderLocation = location;
                   });
                }
                if (!_hasSent)
                {
                    await ServerCommunicator.Instance.SendLocationAsync(MyLocation);
                    _hasSent = true;
                }
            });
        }
        #endregion


        #region Event Subscribers

        private async void OnLocationUpdates(object sender, Location location)
        {
            //Hover on customized "CompareTo" to see how two locations compared.
            if ((MyLocation == null) || (MyLocation.CompareTo(location) != 0))
            {
                Device.BeginInvokeOnMainThread(() =>
                { 
                  MyLocation = location;
                });
                if (ServerCommunicator.Instance.GroupID != null)
                {
                    //TODO: why SendLocationAsync(MyLocation) throwing exception
                    await ServerCommunicator.Instance.SendLocationAsync(location);
                }

            }
        }

        private void OnBackgroundWork()
        {
            //await GeolocationManager.instance.StopUpdatingLocationAsync();
        }

        private async void OnForegroundWork()
        {
            await ServerCommunicator.Instance.SendMemberInfo();

            #region Location Permissions
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
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        IsShowingLocation = true;
                    });
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
            #endregion
        }
        #endregion
    }
}