using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Command;
using FollowMeApp.Model;
using System.Windows.Input;
using Xamarin.Forms.Maps;

namespace FollowMeApp.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class ShareViewModel : ViewModelBase
    {
        private readonly IDeviceService _deviceService;
        private readonly INavigationService _navigationService;
        private DeviceData _deviceData;
        private Position _userCurrentPosition;

        public DeviceData DeviceData
        {
            get
            {
                return _deviceData;
            }
            set
            {
                Set(ref _deviceData, value);
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

        public ShareViewModel() :
            this(new DeviceService(), null)
        {
            GenerateUrlCommand = new RelayCommand(OnGenerateUrlCommand, CanGenerateUrlCommand);
            MessengerInstance.Register<Position>(this, "UserCurrentPosition", position => UserCurrentPosition = position);
            MessengerInstance.Register<DeviceData>(this, "DeviceData", device => DeviceData = device);

        }

        protected ShareViewModel( IDeviceService deviceService, INavigationService navigationService)
        {
            _deviceService = deviceService;
            _navigationService = navigationService;
            
        }

        #region Commands
        public ICommand GenerateUrlCommand { get; private set; }
        private void OnGenerateUrlCommand()
        {

        }
        private bool CanGenerateUrlCommand()
        {
            return true;
        }
        #endregion
    }
}