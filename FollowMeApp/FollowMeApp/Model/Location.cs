﻿using System;

namespace FollowMeApp.Model
{
    public class Location : IComparable<Location>
    {
        public Location() { }

        public Location( double latidue, double longitude, int speed, int heading)
        {
            Latitude = latidue;
            Longitude = longitude;
            Speed = speed;
            Heading = heading;
        }
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int Speed { get; set; }

        public int Heading { get; set; }

        /// <summary>
        /// Has been customized.
        /// <para/><b>Return 0</b> if two Location objects have equally Latitude and Longitude, respectively.
        /// <para/><b>Return -1</b> if either Latitude or Longitude is not equal.
        /// <para/><b>NOTE:</b> Latitudes and Longitudes are rounded up to 4th decimal places just for comparison. The acutual values would not change.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Location other)
        {
            if (other == null) return -1;
            if (Math.Round(Latitude, 4) == Math.Round(other.Latitude, 4) && Math.Round(Longitude, 4) == Math.Round(other.Longitude, 4))
                return 0;

            return -1;
        }
        
        public override string ToString()
        {
            return string.Format("Lat: {0}. Long: {1}. Speed: {2}. Heading: {3}", Latitude, Longitude, Speed, Heading);
        }
    }
}
