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
        private DeviceData _deviceData;
        private Location _location;

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
        
        public ShareViewModel() :
            this(new DeviceService(), null)
        {
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
                    DeviceData = deviceData;
                });
        }

        #region Commands
        public ICommand GenerateUrlCommand { get; private set; }

        
        #endregion
    }
}