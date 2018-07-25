using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using FollowMeApp.Model;
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
    }
}