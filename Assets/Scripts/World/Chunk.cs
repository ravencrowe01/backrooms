using Raven.Backrooms.Framework.Word.Config;
using UnityEngine;

namespace Backrooms.Assets.Scripts.World {
    public class Chunk : MonoBehaviour {
        public struct ChunkRoom {
            public Vector2 Coordinates { get; set; }
            public Room Room { get; set; }
        }

        [SerializeField]
        private int _width;
        [SerializeField]
        private int _height;

        [SerializeField]
        private ChunkRoom[] _chunkRooms;

        private Room[,] _rooms;

        protected Chunk () { }

        public Chunk (IChunkConfig config) {
            Init (config);
        }

        private void Init (IChunkConfig config) {
            _rooms = new Room[config.Width, config.Height];

            for (int x = 0; x < config.Width; x++) {
                for (int y = 0; y < config.Height; y++) {
                    var roomConfig = config.Rooms[x, y];
                    // TODO room database time
                }
            }
        }

        private void Awake () {
            if(_chunkRooms != null & _chunkRooms.Length > 0) {
                _rooms = new Room[_width, _height];

                foreach(var room in _chunkRooms) {
                    _rooms[(int) room.Coordinates.x, (int) room.Coordinates.y] = room.Room;
                }
            }
        }
    }
}
