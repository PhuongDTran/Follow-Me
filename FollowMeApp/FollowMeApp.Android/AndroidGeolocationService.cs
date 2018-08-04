using Android.Gms.Location;
using FollowMeApp.Model;
using System;
using System.Threading.Tasks;

namespace FollowMeApp.Droid
{
    public class AndroidGeolocationService :LocationCallback, IGeolocationService
    {
        private IGeolocationListener _geolocationListener;
        private FusedLocationProviderClient _fusedLocationProviderClient;
        public event EventHandler<Location> LocationUpdatesEvent;

        public AndroidGeolocationService(MainActivity mainActivity)
        {
            _fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient(mainActivity);
        }

        public void AddGeolocationListener( IGeolocationListener listener)
        {
            _geolocationListener = listener;
        }

        public async Task StartUpdatingLocationAsync()
        {
            LocationRequest locationRequest = new LocationRequest()
               .SetPriority(LocationRequest.PriorityHighAccuracy)
               .SetInterval(1000 * 10)
               .SetFastestInterval(1000 * 1);

            await _fusedLocationProviderClient.RequestLocationUpdatesAsync(locationRequest, this);
        }

        public async Task StopUpdatingLocationAsync()
        {
            await _fusedLocationProviderClient.RemoveLocationUpdatesAsync(this);
        }

        public override void OnLocationAvailability(LocationAvailability locationAvailability)
        {
            base.OnLocationAvailability(locationAvailability);
            //TODO: handle situations when location is not available
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
                //TODO: update location only when location changed  
                LocationUpdatesEvent?.Invoke(this, location);

                if (_geolocationListener != null)
                {
                    _geolocationListener.OnLocationUpdated(location);
                }
            }
        }
    }
}