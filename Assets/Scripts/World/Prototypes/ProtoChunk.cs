using Backrooms.Assets.Scripts.World.Config;
using System.Collections.Generic;
using UnityEngine;

namespace Backrooms.Assets.Scripts.World.Prototypes {
    public class ProtoChunk {
        public readonly int Size;

        private ProtoRoom[,] _rooms;

        public IReadOnlyDictionary<Vector2, ProtoHallway> Hallways => _hallways;
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

        public IEnumerable<Vector2> GetOpenRooms (Direction dir)  {
            var rooms = new List<Vector2> ();

            for(int i = 0; i < Size; i++) {
                if (_rooms[GetX(i), GetY(i)].GetOpenSides ()[dir].Open) {
                    rooms.Add (new Vector2 (i, 0));
                }
            }

            return rooms;

            int GetX (int i) {
                if(dir == Direction.North || dir == Direction.South) {
                    return i;
                }
                else {
                    return dir == Direction.East ? 0 : Size - 1;
                }
            }

            int GetY (int i) {
                if (dir == Direction.East || dir == Direction.West) {
                    return i;
                }
                else {
                    return dir == Direction.South ? 0 : Size - 1;
                }
            }
        }

        public Dictionary<Direction, List<ProtoRoom>> GetOpenSides () {
            var states = new Dictionary<Direction, List<ProtoRoom>> () {
                {Direction.North, new List<ProtoRoom>() },
                {Direction.South, new List<ProtoRoom>() },
                {Direction.East, new List<ProtoRoom>() },
                {Direction.West, new List<ProtoRoom>() }
            };

            for (int x = 0; x < Size; x++) {
                for (int y = 0; y < Size; y++) {
                    if (IsEdgeRoom (x, y)) {
                        var open = _rooms[x, y].GetOpenSides ();

                        if (x != 0) {
                            open.Remove (Direction.West);
                        }

                        if (x != Size - 1) {
                            open.Remove (Direction.East);
                        }

                        if (y != 0) {
                            open.Remove (Direction.North);
                        }

                        if (y != Size - 1) {
                            open.Remove (Direction.South);
                        }

                        foreach (var o in open.Keys) {
                            states[o].Add (_rooms[x, y]);
                        }
                    }
                }
            }

            return states;

            bool IsEdgeRoom (int x, int y) => x == 0 && y >= 0 || x >= 0 && y == 0 || x == Size - 1 && y >= 0 || x >= 0 && y == Size - 1;
        }

        public ChunkConfig ToChunkConfig (Vector2 cords) {
            var chunk = new ChunkConfig (cords, _rooms.GetLength (0));

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
