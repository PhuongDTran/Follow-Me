using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace FollowMeApp.Model
{
    public class DataService : IDataService
    {
        public void GetData(Action<DataItem, Exception> callback)
        {
            // Use this to connect to the actual data service

            var item = new DataItem("Follow Me", "Start a Trip");
            callback(item, null);
           
        }
        public async void GetLocation( Action<Location, Exception> callback)
        {
            var currentLocation = await GetLocation();
            callback(currentLocation, null);
        }

        private async Task<Location> GetLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);
                if (location != null)
                {
                    return location;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
            return null;
        }
    }
}