using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
namespace FollowMeApp.Model
{
    class LocationData
    {
        public LocationData(Location location)
        {
            CurrentLocation = location;
        }

        public Location CurrentLocation
        {
            get;
            private set;
        }
    }
}
