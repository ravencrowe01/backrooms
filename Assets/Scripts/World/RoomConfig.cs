namespace Backrooms.Assets.Scripts.World {
    public class RoomConfig {
        public bool North { get; private set; }
        public bool South { get; private set; }
        public bool East { get; private set; }
        public bool West { get; private set; }

        public RoomConfig (bool north, bool south, bool east, bool west) {
            North = north;
            South = south;
            East = east;
            West = west;
        }
    }
}
