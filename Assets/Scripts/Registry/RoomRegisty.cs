using Backrooms.Assets.Scripts.World;
using Backrooms.Assets.Scripts.World.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Backrooms.Assets.Scripts.Database {
    public class RoomRegistry : MonoBehaviour {
        public static RoomRegistry Instance { get; private set; }

        [SerializeField]
        private Room[] _roomArray;

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

        private void Awake () {
            if (Instance != null && Instance != this) {
                Destroy (this);
            }
            else {
                Instance = this;
            }

            if (_roomArray != null && _roomArray.Length > 0) {
                InitRegistry ();
            }
        }

        private void InitRegistry () {
            foreach (var room in _roomArray) {
                _rooms.Add (room.ID, room);
            }

            _roomArray = null;
        }
    }
}
