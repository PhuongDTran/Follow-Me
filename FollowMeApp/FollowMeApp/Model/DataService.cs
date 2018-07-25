using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Plugin.DeviceInfo;

namespace FollowMeApp.Model
{
    public class DataService : IDataService
    {
        public async void GetUserLocation(Action<LocationData, Exception> callback)
        {
            var locationData = new LocationData();
            Location location = null;
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                location = await Geolocation.GetLocationAsync(request);
                locationData.Location = location;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                Console.WriteLine("not supportted on device exception");
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                Console.WriteLine("permission exception");
            }
            catch (Exception ex)
            {
                // Unable to get location
                Console.WriteLine("unable to get location");
            }
            callback(locationData, null);
        }

        public void GetDeviceInfo(Action<DeviceData, Exception> callback)
        {
            //https://github.com/jamesmontemagno/DeviceInfoPlugin
            DeviceData deviceData = new DeviceData()
            {
                DeviceID = CrossDeviceInfo.Current.Id,
                DeviceName = CrossDeviceInfo.Current.DeviceName
            };

        }
    }
}