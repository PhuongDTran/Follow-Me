using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLocation;
using FollowMeApp.Model;
using Foundation;
using UIKit;

namespace FollowMeApp.iOS
{
    public class iOSGeolocationService : IGeolocationService
    {
        private CLLocationManager _locationManager;
        private IGeolocationListener _geolocationListener;
        public event EventHandler<Location> LocationUpdatesEvent;

        public iOSGeolocationService()
        {
            _locationManager = new CLLocationManager
            {
                PausesLocationUpdatesAutomatically = false
            };

            //iOS 8 has additional permisstion requirements
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                _locationManager.RequestAlwaysAuthorization(); //works in background
                _locationManager.RequestWhenInUseAuthorization(); //only in foreground
            }

            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            {
                _locationManager.AllowsBackgroundLocationUpdates = true;
            }
        }
        #region Properties
        public CLLocationManager CLLocationManager
        {
            get { return _locationManager; }
        }
        #endregion

        #region Interface Implementations
        public void AddGeolocationListener(IGeolocationListener listener)
        {
            _geolocationListener = listener;
        }

        public Task StartUpdatingLocationAsync()
        {
            if (CLLocationManager.LocationServicesEnabled)
            {
                Location location = new Location();
                _locationManager.DesiredAccuracy = 1;
                _locationManager.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) =>
                {
                    location.Latitude = e.Locations.Last().Coordinate.Latitude;
                    location.Longitude = e.Locations.Last().Coordinate.Longitude;
                    location.Speed = (int)Math.Round(e.Locations.Last().Speed * 2.23694); // convert to mph
                    // fire a custom event
                    LocationUpdatesEvent?.Invoke(this, location);
                };
                _locationManager.UpdatedHeading += (object sender, CLHeadingUpdatedEventArgs e) =>
                {
                    location.Heading = (int)e.NewHeading.TrueHeading;
                    //LocationUpdatesEvent?.Invoke(this, location);
                };
                _locationManager.StartUpdatingLocation();
                _locationManager.StartUpdatingHeading();
            }
            return Task.CompletedTask;
        }

        public  Task StopUpdatingLocationAsync()
        {
            _locationManager.StopUpdatingLocation();
            return Task.CompletedTask;
        }
        #endregion
    }
}