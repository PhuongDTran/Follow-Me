using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace FollowMeApp.Model
{
    public class DataService : IDataService
    {
        public async void GetUserLocation( Action<Location, Exception> callback)
        {
            Location location = null;
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                location = await Geolocation.GetLocationAsync(request);
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
            callback(location, null);
        }

        /*private async Task<Location> GetLocation()
        {
            Location location = null;
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                location = await Geolocation.GetLocationAsync(request);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
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
            return location;
        }*/
    }
}