
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using FollowMeApp.Model;

namespace FollowMeApp.Droid
{
    [Activity(Label = "FollowMeApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { Intent.ActionView},
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "followme",
        DataHost = "newtrip")]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(bundle);
            Rg.Plugins.Popup.Popup.Init(this,bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            global::Xamarin.FormsMaps.Init(this, bundle);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, bundle);
            GeolocationManager.instance = new AndroidGeolocationService(this);
            ServerCommunicationManager.instance = new ServerCommunication();
            var groupId = Intent?.Data?.EncodedAuthority;
            if ( groupId != null && ServerCommunicationManager.instance.GroupId != null)
            {
                ServerCommunicationManager.instance.GroupId = groupId;
            }

            LoadApplication(new App());
        }

        protected override void OnStart()
        {
            base.OnStart();
            GrantPermissions();

        }

        //TODO: temporarily handle permissions
        private void GrantPermissions()
        {
            if(CheckSelfPermission(Manifest.Permission.AccessCoarseLocation) != (int)Permission.Granted)
            {
                RequestPermissions(new string[] { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation }, 0);
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

