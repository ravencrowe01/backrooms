using Backrooms.Assets.Scripts;
using Backrooms.Assets.Scripts.World;
using Backrooms.Scripts.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Backrooms.Assets.Scripts.Databases {
    public class RoomDatabase : MonoBehaviour {
        /// <summary>
        /// This is exposed so that rooms can be added in the editor,
        /// and kept private to prevent other scripts from accessing it.
        /// </summary>
        [SerializeField]
        private Room[] Rooms;

        private static IDictionary<int, Room> _rooms;

        public static RoomDatabase Instance { get; private set; }

        private void Awake () {
            if (Instance != null & Instance != this) {
                Destroy (this);
            }
            else {
                Instance = this;
            }

            _rooms = new Dictionary<int, Room> ();

            foreach (var room in Rooms) {
                try {
                    _rooms.Add (room.ID, room);
                }
                catch (ArgumentException) {
                    throw new IDConflictException ($"A room with ID {room.ID} already exists in the database.");
                }
            }
        }

        public Room GetRoomByID (int id) => _rooms.ContainsKey (id) ? _rooms[id] : default;

        public Room GetRandomRoomWithOpenSides (IEnumerable<Direction> sides) {
            var temp = _rooms.Values.ToList ();

            foreach (var side in sides) {
                temp = temp.Where (r => r.IsSideOpen (side)).ToList ();
            }

            return temp[Random.Range (0, temp.Count ())];
        }
    }
}
