using FollowMeApp.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using System;
using System.Windows.Input;
using Xamarin.Forms.Maps;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace FollowMeApp.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// The <see cref="Title" /> property's name.
        /// </summary>
        public const string TitlePropertyName = "Title";

        /// <summary>
        /// The <see cref="StartTripText" /> property's name.
        /// </summary>
        public const string StartButtonPropertyText = "StartButtonText";

        private readonly INavigationService _navigationService;
        private Position _userCurrentPosition;
        private Location _location;
        private Location _leaderLocation;
        
        public Location LeaderLocation
        {
            get
            {
                return _leaderLocation;
            }
            set
            {
                Set(ref _leaderLocation, value);
            }
        }

        public Position UserCurrentPosition
        {
            get
            {
                return _userCurrentPosition;
            }
            set
            {
                Set(ref _userCurrentPosition, value);
            }
        }

        public string StartTripText
        {
            get
            {
                return "Start a trip";
            }
        }

        /// <summary>
        /// Sets and gets the WelcomeTitle property.
        /// Changes to this property's value raise the PropertyChanged event.
        /// Use the "mvvminpc*" snippet group to create more such properties.
        /// </summary>
        public string Title
        {
            get
            {
                return "Follow Me";
            }
        }

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
                UserCurrentPosition = new Position(location.Latitude, location.Longitude);
                _location = location;
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
                    var deviceService = new DeviceService();
                    Model.Device deviceData=null;
                    deviceService.GetDeviceData((device, error) =>
                   {
                       deviceData = device;
                   });
                    LeaderLocation = await ServerCommunicator.Instance.SendMemberInfo(deviceData, _location);
                }
            };
        }
    }
}