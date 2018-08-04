namespace FollowMeApp.Model
{
    public class Location
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

        public override string ToString()
        {
            return string.Format("Lat: {0}. Long: {1}. Speed: {2}. Heading: {3}", Latitude, Longitude, Speed, Heading);
        }
    }
}
