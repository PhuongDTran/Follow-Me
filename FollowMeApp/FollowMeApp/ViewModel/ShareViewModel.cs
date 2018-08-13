using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Command;
using FollowMeApp.Model;
using System.Windows.Input;
using Xamarin.Forms.Maps;
using System;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
        private ServerCommunication _serverCommunication;
        private Device _deviceData;
        private String _groupId;
        private Location _location;

        public String GroupId
        {
            get
            {
                return _groupId;
            }
            set
            {
                Set(ref _groupId, value);
            }
        }
        
        public ShareViewModel() :
            this(new DeviceService(), null)
        {
            _serverCommunication = new ServerCommunication();
            GenerateUrlCommand = new RelayCommand(async() => await OnGenerateUrlCommand(), CanGenerateUrlCommand);
            GeolocationManager.instance.LocationUpdatesEvent += (sender, location) =>
            {
                _location = location;
            };
        }
        
        protected ShareViewModel( IDeviceService deviceService, INavigationService navigationService)
        {
            _deviceService = deviceService;
            _navigationService = navigationService;
            _deviceService.GetDeviceData(
                (deviceData, error) =>
                {
                    _deviceData = deviceData;
                });
        }

        #region Commands
        public ICommand GenerateUrlCommand { get; private set; }

        private async Task OnGenerateUrlCommand()
        {
            GroupId = await _serverCommunication.RequestGroupId(_deviceData, _location);
        }
  
        private bool CanGenerateUrlCommand()
        {
            return true;
        }
        #endregion
    }
}