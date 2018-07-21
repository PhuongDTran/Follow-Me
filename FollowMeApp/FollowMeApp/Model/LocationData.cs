using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace FollowMeApp.Model
{
    public class LocationData
    {
        public LocationData() { }

        public LocationData (Location location, Double speed, Double heading)
        {
            Location = location;
            Speed = speed;
            Heading = heading;
        }
        public Location Location { get; set; }
   
        public Double Speed { get; set; }
       
        public Double Heading { get; set; }
        
    }
}
