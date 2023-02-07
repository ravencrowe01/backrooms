using Backrooms.Assets.Scripts.World;
using Backrooms.Assets.Scripts.World.Config;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Backrooms.Assets.Scripts.Database {
    public class RoomRegistry {
        public static RoomRegistry Instance {
            get {
                if (_instance == null) {
                    lock (_instanceLock) {
                        _instance ??= new RoomRegistry ();
                    }
                }

                return _instance;
            }
        }

        private static RoomRegistry _instance;
        private static object _instanceLock = new object ();

        public IReadOnlyDictionary<ID, Room> Rooms => _rooms;
        private Dictionary<ID, Room> _rooms;

        private RoomRegistry () {
            _rooms = new Dictionary<ID, Room> ();
        }

        public void AddRoom (ID id, Room room) => _rooms.Add (id, room);

        public Room GetRoom (ID id) => _rooms[id];

        public IEnumerable<Room> FilterRooms (IDictionary<Direction, ISideStateConfig> filter) =>
            _rooms.Values.Where (r => {
                foreach (var dir in Enum.GetValues (typeof (Direction)).Cast<Direction> ()) {
                    if (!filter[dir].Compare (r.SideStates[dir])) {
                        return false;
                    }
                }

                return true;
            });

    }
}
