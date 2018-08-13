using Plugin.DeviceInfo;
using System;

namespace FollowMeApp.Model
{
    public class DeviceService : IDeviceService
    {
        public void GetDeviceData(Action<Device, Exception> callback)
        {
            //https://github.com/jamesmontemagno/DeviceInfoPlugin
            Device deviceData = new Device()
            {
                DeviceID = CrossDeviceInfo.Current.Id,
                DeviceName = CrossDeviceInfo.Current.DeviceName,
                Platform = CrossDeviceInfo.Current.Platform.ToString()
            };
            callback(deviceData, null);
        }
    }
}
