namespace OrderEntryEngine
{
    public class LocationEventArgs
    {
        public LocationEventArgs(Location location)
        {
            this.Location = location;
        }

        public Location Location { get; private set; }
    }
}