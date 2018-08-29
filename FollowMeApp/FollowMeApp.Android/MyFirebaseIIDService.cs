using Android.App;
using Android.Util;
using Firebase.Iid;
using FollowMeApp.Model;

namespace FollowMeApp.Droid
{

    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseIIDService : FirebaseInstanceIdService
    {
        /// <summary>
        /// This Class is not run on Main Thread
        /// </summary>
        const string TAG = "MyFirebaseIIDService";
        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            Log.Debug(TAG, "Refreshed token: " + refreshedToken);
            SendRegistrationToServer(refreshedToken);
        }
        void SendRegistrationToServer(string token)
        {
           ServerCommunicator.Instance.SendTokenAsync(token);   
        }
    }
}