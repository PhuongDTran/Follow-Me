﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FollowMeApp.Model
{
    public interface IDeviceService
    {
        void GetDeviceData(Action<DeviceData, Exception> callback);
    }
}