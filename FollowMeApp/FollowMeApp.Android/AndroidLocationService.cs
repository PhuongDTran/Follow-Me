using Android.Gms.Location;
using FollowMeApp.Model;
using System;
using System.Threading.Tasks;
using FollowMeApp.Model;
namespace FollowMeApp.Droid
{
    public class AndroidLocationService :LocationCallback, IGeolocationService
    {
        IGeolocationListener _geolocationListener;
        FusedLocationProviderClient _fusedLocationProviderClient;
        
        public AndroidLocationService(MainActivity mainActivity)
        {
            _fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient(mainActivity);
        }

        public void AddGeolocationListener( IGeolocationListener listener)
        {
            _geolocationListener = listener;
        }

        public void StartLocationUpdates()
        {
          
        }

        public async Task StartUpdatingLocationAsync()
        {
            LocationRequest locationRequest = new LocationRequest()
               .SetPriority(LocationRequest.PriorityHighAccuracy)
               .SetInterval(1000 * 10)
               .SetFastestInterval(1000 * 1);

            await _fusedLocationProviderClient.RequestLocationUpdatesAsync(locationRequest, this);
        }

        public void StopUpdatingLocation(IGeolocationListener listener)
        {
        }

        public override void OnLocationAvailability(LocationAvailability locationAvailability)
        {
            base.OnLocationAvailability(locationAvailability);
        }

        public override void OnLocationResult(LocationResult result)
        {
            base.OnLocationResult(result);
            if (result.LastLocation != null)
            {
                Location location = new Location()
                {
                    Latitude = result.LastLocation.Latitude,
                    Longitude = result.LastLocation.Longitude,
                    Speed = (int)Math.Round(result.LastLocation.Speed * 2.23694) // convert to mph
                };

                if(_geolocationListener != null) _geolocationListener.OnLocationUpdated(location);
                Console.WriteLine(location.ToString());
            }
        }
    }
}