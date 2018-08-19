using FollowMeApp.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace FollowMeApp.ViewModel
{

    public class MainViewModel : ViewModelBase
    {

        private readonly INavigationService _navigationService;
        private Location _myLocation;
        private Location _leaderLocation;
        private IDictionary<string, Location> _members;

        #region Properties

        public IDictionary<string,Location> Members
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
        public ICommand CurrentLocationCommand { get; private set; }

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
           
            GeolocationManager.instance.LocationUpdatesEvent += (object sender, Location location) =>
            {
                MyLocation = location;   
            };
            
            //listen for AppStart event and run the code very early when app started
            ((App)Application.Current).AppStart += async () =>
            {
                await GeolocationManager.instance.StartUpdatingLocationAsync();
            };

            ServerCommunicator.Instance.PropertyChanged += async (s, e) =>
            {
                if(e.PropertyName == nameof(ServerCommunicator.Instance.GroupId))
                {
                    //TODO: deviceService created in MainVM and ShareVM.
                    var deviceService = new DeviceService();
                    Model.Device deviceData=null;
                    deviceService.GetDeviceData((device, error) =>
                   {
                       deviceData = device;
                   });
                   LeaderLocation = await ServerCommunicator.Instance.SendMemberInfo(deviceData, _myLocation);
                }
            };

        }
    }
}