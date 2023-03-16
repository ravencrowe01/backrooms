using Backrooms.Assets.Scripts.Database;
using Backrooms.Assets.Scripts.World.Config;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Backrooms.Assets.Scripts.World {
    public class Chunk : MonoBehaviour {
        public ID ID { get; private set; }
        [SerializeField]
        private int _idM;

        public Vector2 Coordinates => _cords;
        private Vector2 _cords;

        [SerializeField]
        private int _width;
        [SerializeField]
        private int _height;

        private Room[,] _rooms;

        [SerializeField]
        private int _chunkSize = 16;

        public IReadOnlyDictionary<Direction, List<IRoomConfig>> OpenConnections;

        public IReadOnlyList<IHallwayConfig> Hallways;

        public GameObject ChunkColumn;
        public GameObject RoomHolder;

        protected Chunk () { }

        public void Init (IChunkConfig config, int seed) {
            _width = config.Width;
            _height = config.Height;

            _cords = new Vector2 (config.Coordinates.x, config.Coordinates.y);

            _rooms = new Room[config.Width, config.Height];

            OpenConnections = config.GetOpenSides ();

            Hallways = config.Hallways;

            BuildRoomsHolder ();

            for (int x = 0; x < config.Width; x++) {
                for (int z = 0; z < config.Height; z++) {
                    Random.InitState (seed ^ x ^ z);

                    var roomConfig = config.Rooms[x, z];
                    var found = RoomRegistry.Instance.FilterRooms ((IDictionary<Direction, ISideStateConfig>) roomConfig.SideStates).ToList ();
                    var roll = Random.Range (0, found.Count ());

                    _rooms[x, z] = found[roll];
                }
            }
        }

        private void BuildRoomsHolder () {
            for (int x = 0; x < _width; x++) {
                var c = Instantiate (ChunkColumn, this.transform);

                c.transform.position = new Vector3 (x * _chunkSize, 0, 0);
            }
        }

        public void InstantiateRooms () {
            for (int x = 0; x < _width; x++) {
                for (int z = 0; z < _height; z++) {
                    var trans = transform.GetChild (x);
                    var room = Instantiate (_rooms[x, z], trans);

                    room.transform.position = new Vector3 (trans.position.x, 0, z * _chunkSize);
                }
            }
        }
    }
}
