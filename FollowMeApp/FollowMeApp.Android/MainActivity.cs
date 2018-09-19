using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Firebase.Messaging;
using FollowMeApp.Model;
using Plugin.Permissions;
using Messenger = GalaSoft.MvvmLight.Messaging.Messenger;

namespace FollowMeApp.Droid
{
    [Activity(Label = "FollowMeApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { Intent.ActionView},
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "followme",
        DataHost = "newtrip")]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, ActivityCompat.IOnRequestPermissionsResultCallback
    {
        internal static readonly string CHANNEL_ID = "my_notification_channel";
        internal static readonly string TAG = "MainActivity";
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(bundle);

            #region plugins intialized
            Rg.Plugins.Popup.Popup.Init(this,bundle);
            Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, bundle);
            #endregion

            #region Geolocation service
            if (GeolocationManager.instance == null)
            {
                GeolocationManager.instance = new AndroidGeolocationService(this);
            }
            #endregion

            #region url scheme
            var groupId = Intent?.Data?.GetQueryParameter("groupid");
            var leaderId = Intent?.Data?.GetQueryParameter("leaderid");
            if ( groupId != null && leaderId != null)
            {
                ServerCommunicator.Instance.GroupID = groupId;
                Messenger.Default.Send(leaderId, PublishedData.GroupIdNotification);
            }
            #endregion
      
            CreateNotificationChannel();

            LoadApplication(new App());
        }

        /// <summary>
        /// Callback received when a permissions request has been completed.
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="permissions"></param>
        /// <param name="grantResults"></param>
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        /// <summary>
        /// Check if Google Play Services APK is installed
        /// </summary>
        /// <returns></returns>
        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    // msgText.Text = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                }
                else
                {
                   // msgText.Text = "This device is not supported";
                    Finish();
                }
                return false;
            }
            else
            {
                //msgText.Text = "Google Play Services is available.";
                return true;
            }
        }


        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channel = new NotificationChannel(CHANNEL_ID, "FCM Notifications", NotificationImportance.Default)
            {
                Description = "Firebase Cloud Messages appear in this channel"
            };

            var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
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

