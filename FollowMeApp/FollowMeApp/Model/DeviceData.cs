using System;

namespace FollowMeApp.Model
{
    public class DeviceData
    {
        public DeviceData() { }

        public DeviceData(String deviceId, String deviceName)
        {
            DeviceID = deviceId;
            DeviceName = deviceName;
        }

        public String DeviceID { get; set; }

        public String DeviceName { get; set; }
    }
}
