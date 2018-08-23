
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.Design.Widget;
using Android.Widget;
using Android.Util;
using FollowMeApp.Model;

namespace FollowMeApp.Droid
{
    [Activity(Label = "FollowMeApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { Intent.ActionView},
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "followme",
        DataHost = "newtrip")]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, ActivityCompat.IOnRequestPermissionsResultCallback
    {
        //Id to identify location permissions request.
        static readonly int REQUEST_LOCATION = 0;
        //permissions required to get location.
        static readonly string[] PERMISSIONS_LOCATION = { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation };
        static readonly string TAG = "MainActivity";

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(bundle);

            //plugins intialized
            Rg.Plugins.Popup.Popup.Init(this,bundle);
            Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, bundle);

            if (GeolocationManager.instance == null)
            {
                GeolocationManager.instance = new AndroidGeolocationService(this);
            }
           
            var groupId = Intent?.Data?.GetQueryParameter("groupid");
            if ( groupId != null)
            {
                ServerCommunicator.Instance.GroupId = groupId;
                
            }
            LoadApplication(new App());
        }

        protected override void OnStart()
        {
            base.OnStart();
            RequestLocationPermissions();

        }
        
        void RequestLocationPermissions()
        {
            if( ActivityCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) != (int)Permission.Granted
                || ActivityCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, PERMISSIONS_LOCATION, REQUEST_LOCATION);
            }
        }
        
     	//Callback received when a permissions request has been completed.
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            
            if(requestCode == REQUEST_LOCATION)
            {
                if(grantResults.Length == 2 && grantResults[0] == Permission.Granted && grantResults[1] == Permission.Granted)
                {
                    Log.Info(TAG, "Location permission has now been granted.");
                }
                else
                {
                    Log.Info(TAG, "Location permission was NOT granted.");
                }
            }
            else
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
        }


        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                // Do something if there are some pages in the `PopupStack`
            }
            else
            {
                // Do something if there are not any pages in the `PopupStack`
            }
        }
    }
}

