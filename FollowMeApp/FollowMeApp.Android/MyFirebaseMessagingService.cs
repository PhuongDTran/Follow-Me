using Android.App;
using Android.Content;
using Android.Util;
using Firebase.Messaging;
using FollowMeApp.Model;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
namespace FollowMeApp.Droid
{

    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMsgService";

        public override void OnMessageReceived(RemoteMessage message)
        {
            IDictionary<string, string> data = message.Data;

            if (data.TryGetValue("leader", out string leaderId))
            {
                Messenger.Default.Send(leaderId, PublishedData.GroupIdNotification);
                Log.Debug(TAG, "Leader has moved");
            }
            else if (data.TryGetValue("member", out string memberId))
            {
                Messenger.Default.Send(memberId, PublishedData.MemberLocationNotification);
                Log.Debug(TAG, "message content:" + memberId);
            }
        }
    }
}