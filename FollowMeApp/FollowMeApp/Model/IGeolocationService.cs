using System;
using System.Collections.Generic;
using System.Text;

namespace FollowMeApp.Model
{
    public interface IGeolocationService
    {
        void StartUpdatingLocation( IGeolocationListener listener);
        void StopUpdatingLocation( IGeolocationListener listener);
    }
}
