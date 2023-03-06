using Backrooms.Assets.Scripts.World.Config;
using System.Collections.Generic;
using UnityEngine;

namespace Backrooms.Assets.Scripts.World.Prototypes {
    public class ProtoChunk {
        public readonly int Size;

        private ProtoRoom[,] _rooms;

        private Dictionary<Vector2, ProtoHallway> _hallways;

        public ProtoChunk (int size) {
            Size = size;
            _rooms = new ProtoRoom[Size, Size];
            _hallways = new Dictionary<Vector2, ProtoHallway> ();
        }

        public void AddRoom (Vector2 cords, ProtoRoom room) => _rooms[(int) cords.x, (int) cords.y] = room;

        public ProtoRoom GetRoom (Vector2 cords) => _rooms[(int) cords.x, (int) cords.y];

        public void SetRoomSideTotalState (Vector2 cords, Direction dir, bool state) => _rooms[(int) cords.x, (int) cords.y].SetSideTotalState (dir, state);

        public void SetRoomSideState (Vector2 cords, Direction side, int i, bool state) => _rooms[(int) cords.x, (int) cords.y].SetSideState (side, i, state);

        public ChunkConfig ToChunkConfig (Vector2 cords) {
            var chunk = new ChunkConfig (cords, _rooms.GetLength (0), _rooms.GetLength (1));

            foreach (var hallway in _hallways.Keys) {
                chunk.AddHallway (hallway, _hallways[hallway].Direction, _hallways[hallway].BuildChance);
            }

            for (int x = 0; x < _rooms.GetLength (0); x++) {
                for (int y = 0; y < _rooms.GetLength (1); y++) {
                    var room = _rooms[x, y];

                    chunk.SetRoom (x, y, room.ToRoomConfig ());
                }
            }

            return chunk;
        }
    }
}
