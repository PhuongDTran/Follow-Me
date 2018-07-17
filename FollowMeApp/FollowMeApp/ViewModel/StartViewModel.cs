using FollowMeApp.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using Xamarin.Forms.Maps;

namespace FollowMeApp.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class StartViewModel : ViewModelBase
    {
        /// <summary>
        /// The <see cref="Title" /> property's name.
        /// </summary>
        public const string TitlePropertyName = "Title";

        /// <summary>
        /// The <see cref="StartButtonText" /> property's name.
        /// </summary>
        public const string StartButtonPropertyText = "StartButtonText";

        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;
        private string _title = "";
        private string _startButtonText = "";
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
            }
        }

        public string StartButtonText
        {
            get
            {
                return _startButtonText;
            }
            set
            {
                Set(ref _startButtonText, value);
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
                return _title;
            }
            set
            {
                Set(ref _title, value);
            }
        }

        public StartViewModel() :
           this(new DataService(), null)
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
        protected StartViewModel(IDataService dataService, INavigationService navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;

            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }

                    Title = item.Title;
                    StartButtonText = item.StartButtonText;
                });

            _dataService.GetUserLocation(
                (location, error) =>
                {
                    if (error != null)
                    {
                        return;
                    }
                    UserCurrentPosition = new Position(location.Latitude, location.Longitude);
                });
        }
    }
}