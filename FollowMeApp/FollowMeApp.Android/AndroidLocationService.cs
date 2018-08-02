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
using FollowMeApp.Model;
namespace FollowMeApp.Droid
{
    public class AndroidLocationService : ILocationService
    {
        public void StartUpdatingLocation(ILocationListener listener)
        {
        }

        public void StopUpdatingLocation(ILocationListener listener)
        {
        }
    }
}