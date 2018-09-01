namespace FollowMeApp.Model
{
    public class PublishedData
    {
        /// <summary>
        /// Radius, in meters, of an overlay circle on each Pin
        /// </summary>
        public static readonly double PinOverlayRadius = 100;

        /// <summary>
        /// A Token used by Galasoft.Mvvmlight.messeger to notify to subsribers when a groupid abtained from URL scheme.
        /// </summary>
        public static readonly string GroupIdNotification = "groupid_received";

        /// <summary>
        /// <para>A Token used by Galasoft.Mvvmlight.messeger to notify to subsribers when a member id received from app server.</para>
        /// <para>app server send out member id of any members has location changed to group leader.</para>
        /// <para>group leader uses member id to update member's locations.</para>
        /// </summary>
        public static readonly string MemberLocationNotification = "memberid_received";
    }
}
