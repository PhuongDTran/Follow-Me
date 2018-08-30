using Android.App;
using Android.Content;
using Android.Util;
using System.Collections.Generic;
using Firebase.Messaging;
using FollowMeApp.Model;
namespace FollowMeApp.Droid
{

    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMsgService";
        
        public override void OnMessageReceived(RemoteMessage message)
        {
            Log.Debug(TAG, "From: " + message.From);
            IDictionary<string,string> data = message.Data;
            if (data.TryGetValue("member", out string value))
            {
                Log.Debug(TAG, "message content:" + value);
            }
        }
    }
}