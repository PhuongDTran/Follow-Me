using System;
using System.Collections.Generic;
using System.Text;

namespace FollowMeApp.Model
{
    public class GeolocationManager
    {
        public static IGeolocationService instance;

        private GeolocationManager() { }
    }
}
