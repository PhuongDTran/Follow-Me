using System;

namespace FollowMeApp.Model
{
    public class DeviceInfo
    {
        public DeviceInfo() { }
        public DeviceInfo(String deviceId, String deviceName)
        {
            DeviceID = deviceId;
            DeviceName = deviceName;
        }
        public String DeviceID { get; set; }
        public String DeviceName { get; set; }
    }
}
