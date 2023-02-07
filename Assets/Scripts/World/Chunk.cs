using Backrooms.Assets.Scripts.Database;
using Backrooms.Assets.Scripts.RNG;
using Backrooms.Assets.Scripts.World.Config;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Backrooms.Assets.Scripts.World {
    public class Chunk : MonoBehaviour {
        public struct ChunkRoom {
            public Vector2 Coordinates { get; set; }
            public Room Room { get; set; }
        }

        public ID ID { get; private set; }
        [SerializeField]
        private int _idM;

        public Vector2 Coordinates => _cords;
        private Vector2 _cords;

        [SerializeField]
        private int _width;
        [SerializeField]
        private int _height;

        [SerializeField]
        private ChunkRoom[] _chunkRooms;

        private Room[,] _rooms;

        protected Chunk () { }

        public Chunk (IChunkConfig config, IRNG rng) {
            Init (config, rng);
        }

        private void Init (IChunkConfig config, IRNG rng) {
            _width = config.Width;
            _height = config.Height;

            _cords = new Vector2 (config.Coordinates.X, config.Coordinates.Y);

            _rooms = new Room[config.Width, config.Height];

            for (int x = 0; x < config.Width; x++) {
                for (int y = 0; y < config.Height; y++) {
                    var roomConfig = config.Rooms[x, y];
                    var found = RoomRegistry.Instance.FilterRooms ((IDictionary<Direction, ISideStateConfig>) roomConfig.SideStates).ToList();

                    _rooms[x, y] = found[rng.Next (found.Count ())];
                }
            }
        }

        private void Awake () {
            if (_chunkRooms != null & _chunkRooms.Length > 0) {
                ID = new ID (_idM);

                _rooms = new Room[_width, _height];

                foreach(var room in _chunkRooms) {
                    int x = (int) room.Coordinates.x, y = (int) room.Coordinates.y;

                    _rooms[x, y] = room.Room;
                }
            }

            InstantiateRooms ();
        }

        private void InstantiateRooms() {
            for(int x = 0; x < _width; x++) {
                for(int y = 0; y < _height; y++) {
                    Instantiate (_rooms[x, y], transform.GetChild (x).GetChild (y));
                }
            }
        }
    }
}
