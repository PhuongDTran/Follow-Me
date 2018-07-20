using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace FollowMeApp.Model
{
    class LocationData
    {
        public Location Location
        {
            get;
            private set;
        }

        public Double Speed
        {
            get;
            private set;
        }
        public Double Heading
        {
            get;
            private set;
        } 
    }
}
