using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Common;
using Android.Gms.Location;
using FollowMeApp.Model;

namespace FollowMeApp.Droid
{
    public class AndroidLocationService : LocationCallback, IGeolocationService
    {
        private IGeolocationListener[] _listeners;
       
        public AndroidLocationService()
        { 
        }

       
        public void StartUpdatingLocation(IGeolocationListener listener)
        {
        }

        public void StopUpdatingLocation(IGeolocationListener listener)
        {
        }
    }
}