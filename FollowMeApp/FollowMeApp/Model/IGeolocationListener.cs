﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FollowMeApp.Model
{
    public interface IGeolocationListener
    {
        void OnLocationUpdated(Location newLocation);
        void OnLocationPermissionsChanged();
    }
}
