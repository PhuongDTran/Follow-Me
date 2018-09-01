using System.Collections.Generic;
using Xamarin.Forms.Maps;

namespace FollowMeApp.View
{
    public class CustomMap : Map
    {
        public event System.EventHandler<IList<CirclePin>> PinsCleared;

        /// <summary>
        /// Must call this method to remove CirlcePin objects. Otherwise, overlays would not be removed.
        /// </summary>
        public void ClearCirclePins()
        {
            var circlePins = new CirclePin[Pins.Count];
            Pins.CopyTo(circlePins, 0);
            Pins.Clear();
            PinsCleared?.Invoke(this, circlePins);
        }
    }
}
