using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace FollowMeApp.ViewModel
{
    class LocationEvent: EventArgs
    {
        public Location UserLocation { get; set; }
        
    }
}
