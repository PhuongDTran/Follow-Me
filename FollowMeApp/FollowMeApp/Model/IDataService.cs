using System;

namespace FollowMeApp.Model
{
    public interface IDataService
    {
        void GetUserLocation(Action<LocationData, Exception> callback);
        void GetDeviceInfo(Action<DeviceData, Exception> callback); 
    }
}