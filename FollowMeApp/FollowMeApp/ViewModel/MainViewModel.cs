using FollowMeApp.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using System;
using System.Windows.Input;
using Xamarin.Forms.Maps;
using System.Threading.Tasks;

namespace FollowMeApp.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase, IGeolocationListener
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

        public Position UserCurrentPosition
        {
            get
            {
                return _userCurrentPosition;
            }
            set
            {
                Set(ref _userCurrentPosition, value);
                MessengerInstance.Send(_userCurrentPosition, PublishedData.CurrentPositionToken);
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
            GeolocationManager.instance.AddGeolocationListener(this);
        }
        
        public void OnLocationUpdated(Location newLocation)
        {
            UserCurrentPosition = new Position(newLocation.Latitude, newLocation.Longitude);
        }

        public void OnLocationPermissionsChanged()
        {
        }
    }
}