using Android.Gms.Location;
using FollowMeApp.Model;
using System;
using System.Threading.Tasks;
using Android.Gms.Common;
using Android.Util;

namespace FollowMeApp.Droid
{
    public class AndroidGeolocationService : LocationCallback, IGeolocationService
    {
        private IGeolocationListener _geolocationListener;
        private FusedLocationProviderClient _fusedLocationProviderClient;
        private readonly MainActivity _mainActivity;
        public event EventHandler<Location> LocationUpdatesEvent;

        public AndroidGeolocationService(MainActivity mainActivity)
        {
            _mainActivity = mainActivity;
            _fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient(_mainActivity);
        }

        public void AddGeolocationListener(IGeolocationListener listener)
        {
            _geolocationListener = listener;
        }

        public async Task StartUpdatingLocationAsync()
        {
            if (IsGooglePlayServicesInstalled())
            {
                LocationRequest locationRequest = new LocationRequest()
                   .SetPriority(LocationRequest.PriorityHighAccuracy)
                   .SetInterval(1000 * 10)
                   .SetFastestInterval(1000 * 1);

                await _fusedLocationProviderClient.RequestLocationUpdatesAsync(locationRequest, this);
            }
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
                    Speed = (int)Math.Round(result.LastLocation.Speed * 2.23694), // convert to mph
                    Heading = (int)result.LastLocation.Bearing
                };
                //TODO: update location only when location changed  
                LocationUpdatesEvent?.Invoke(this, location);

                if (_geolocationListener != null)
                {
                    _geolocationListener.OnLocationUpdated(location);
                }
            }
        }

        /// <summary>
        /// Check if Google Play Services installed
        /// </summary>
        /// <returns></returns>
        private bool IsGooglePlayServicesInstalled()
        {
            var queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(_mainActivity);
            if (queryResult == ConnectionResult.Success)
            {
                Log.Info("MainActivity", "Google Play Services is installed on this device.");
                return true;
            }

            if (GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
            {
                // Check if there is a way the user can resolve the issue
                var errorString = GoogleApiAvailability.Instance.GetErrorString(queryResult);
                Log.Error("MainActivity", "There is a problem with Google Play Services on this device: {0} - {1}",
                          queryResult, errorString);

                // Alternately, display the error to the user.
            }

            return false;
        }

    }
}