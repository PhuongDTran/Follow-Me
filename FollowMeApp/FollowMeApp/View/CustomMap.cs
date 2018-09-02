using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms.Maps;

namespace FollowMeApp.View
{
    public class CustomMap : Map
    {
        public ObservableCollection<Position> RouteCoordinates { get; set; }
        public event System.EventHandler<IList<CirclePin>> PinsCleared;

        public CustomMap()
        {
            RouteCoordinates = new ObservableCollection<Position>();
        }

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
