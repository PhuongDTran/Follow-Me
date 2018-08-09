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
            String groupId = "";
            String url = "http://192.168.4.146:4567/groupid/";
            String contentType = "application/json";
            JObject json = new JObject();
            json.Add("id", _deviceData.DeviceID);
            json.Add("name", _deviceData.DeviceName);
            json.Add("lat", _location.Latitude);
            json.Add("lon", _location.Longitude);
            json.Add("speed", _location.Speed);
            json.Add("heading", _location.Heading); //TODO: need heading
            json.Add("platform", _deviceData.Platform);
            HttpClient client = new HttpClient();
            try
            {
                var response = await client.PostAsync(url, new StringContent(json.ToString(), Encoding.UTF8, contentType));
                groupId = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Responsed Group Id:" + groupId);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine("The request was null. ", ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Already sent by the HttpClient instance.", ex.Message);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Underlying issue:network connectivity, DNS failure, or timeout.", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            GroupId = groupId;
        }
  
        private bool CanGenerateUrlCommand()
        {
            return true;
        }
        #endregion
    }
}