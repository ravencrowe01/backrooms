﻿using Backrooms.Assets.Scripts.Database;
using Backrooms.Assets.Scripts.RNG;
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

        public GameObject ChunkColumn;
        public GameObject RoomHolder;

        protected Chunk () { }

        public void Init (IChunkConfig config, IRNG rng) {
            _width = config.Width;
            _height = config.Height;

            _cords = new Vector2 (config.Coordinates.X, config.Coordinates.Y);

            _rooms = new Room[config.Width, config.Height];

            BuildRoomsHolder ();

            for (int x = 0; x < config.Width; x++) {
                for (int z = 0; z < config.Height; z++) {
                    var roomConfig = config.Rooms[x, z];
                    var found = RoomRegistry.Instance.FilterRooms ((IDictionary<Direction, ISideStateConfig>) roomConfig.SideStates).ToList();
                    var roll = rng.Next (found.Count ());

                    _rooms[x, z] = found[roll];
                }
            }
        }

        private void BuildRoomsHolder () {
            for(int x = 0; x < _width; x++) {
                var c = Instantiate (ChunkColumn, this.transform);
                var newPos = new Vector3 (x * 16, 0, 0);
                c.transform.localPosition = newPos;
            }
        }

        public void InstantiateRooms() {
            for(int x = 0; x < _width; x++) {
                for(int z = 0; z < _height; z++) {
                    var trans = transform.GetChild (x);
                    var room = Instantiate (_rooms[x, z], trans);

                    room.transform.position = new Vector3 (trans.position.x, 0, z * 16);
                }
            }
        }
    }
}
