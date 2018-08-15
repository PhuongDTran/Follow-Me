using System;
using System.Collections.Generic;
using System.Text;

namespace FollowMeApp.Model
{
    public sealed class GeolocationManager
    {
        public static IGeolocationService instance;

        private GeolocationManager() { }
    }
}
