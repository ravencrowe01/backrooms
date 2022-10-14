using Backrooms.Scripts.Exceptions;
using Backrooms.Scripts.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Backrooms.Scripts.Databases {
    public class RoomDatabase : MonoBehaviour {
        /// <summary>
        /// This is exposed so that rooms can be added in the editor,
        /// and kept private to prevent other scripts from accessing it.
        /// </summary>
        [SerializeField]
        private Room[] Rooms;

        private IDictionary<int, Room> _rooms;

        public static RoomDatabase Instance { get; private set; }

        private void Awake () {
            if(Instance != null & Instance != this) {
                Destroy (this);
            }
            else {
                Instance = this;
            }

            _rooms = new Dictionary<int, Room> ();

            foreach(var room in Rooms) {
                try {
                    _rooms.Add (room.ID, room);
                }
                catch (ArgumentException) {
                    throw new IDConflictException ();
                }
            }
        }
    }
}
