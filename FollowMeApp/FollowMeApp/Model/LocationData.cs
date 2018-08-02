using System;
using Xamarin.Essentials;

namespace FollowMeApp.Model
{
    public class LocationData
    {
        public LocationData() { }

        public LocationData ( Xamarin.Essentials.Location location, Double speed, Double heading)
        {
            Location = location;
            Speed = speed;
            Heading = heading;
        }
        public Xamarin.Essentials.Location Location { get; set; }
   
        public Double Speed { get; set; }
       
        public Double Heading { get; set; }
        
    }
}
