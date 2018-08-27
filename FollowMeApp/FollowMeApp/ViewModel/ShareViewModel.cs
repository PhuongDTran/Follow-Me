using FollowMeApp.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FollowMeApp.ViewModel
{

    public class ShareViewModel : ViewModelBase
    {
        private readonly IDeviceService _deviceService;
        private readonly INavigationService _navigationService;
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
            GenerateUrlCommand = new RelayCommand(async () => await OnGenerateUrlCommand(), CanGenerateUrlCommand);
            GeolocationManager.instance.LocationUpdatesEvent += (sender, location) =>
            {
                _location = location;
            };
        }

        protected ShareViewModel(IDeviceService deviceService, INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        #region Commands
        public ICommand GenerateUrlCommand { get; private set; }

        private async Task OnGenerateUrlCommand()
        {
            GroupId = await ServerCommunicator.Instance.RequestGroupIdAsync(_location);
        }

        private bool CanGenerateUrlCommand()
        {
            return true;
        }
        #endregion
    }
}