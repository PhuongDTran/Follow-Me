using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FollowMeApp.Model
{
    public interface IGeolocationService
    {
        event EventHandler<Location> LocationUpdatesEvent;
        Task StartUpdatingLocationAsync();
        Task StopUpdatingLocationAsync();
        void AddGeolocationListener( IGeolocationListener listener);
    }
}
