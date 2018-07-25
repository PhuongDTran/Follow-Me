using Plugin.DeviceInfo;
using System;

namespace FollowMeApp.Model
{
    public class DeviceService : IDeviceService
    {
        public void GetDeviceData(Action<DeviceData, Exception> callback)
        {
            //https://github.com/jamesmontemagno/DeviceInfoPlugin
            DeviceData deviceData = new DeviceData()
            {
                DeviceID = CrossDeviceInfo.Current.Id,
                DeviceName = CrossDeviceInfo.Current.DeviceName
            };
            callback(deviceData, null);
        }
    }
}
