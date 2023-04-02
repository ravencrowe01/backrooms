using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

namespace Backrooms.Assets.Scripts.World.Config {
    public class ChunkConfig : IChunkConfig {
        public Vector2 Coordinates => new Vector2 (_cords.x, _cords.y); 
        private Vector2 _cords;

        public int Size { get; private set; }

        public IRoomConfig[,] Rooms => (IRoomConfig[,]) _rooms.Clone ();
        private IRoomConfig[,] _rooms;

        public IReadOnlyList<IHallwayConfig> Hallways => _hallways;
        private List<IHallwayConfig> _hallways;

        public ChunkConfig (Vector2 cords, IRoomConfig[,] rooms, List<IHallwayConfig> hallways) {
            _cords = cords;
            Size = rooms.GetLength (0);
            _rooms = rooms;
            _hallways = hallways;
        }

        public ChunkConfig (Vector2 cords, int size) {
            _cords = cords;
            Size = size;
            _rooms = new IRoomConfig[size, size];
            _hallways = new List<IHallwayConfig> ();
        }

        public void SetRoom (int x, int y, IRoomConfig room) => _rooms[x, y] = room;

        public void AddHallway (Vector2 origin, Direction dir, float chance) => _hallways.Add (new HallwayConfig (origin, dir, chance));

        public Dictionary<Direction, List<IRoomConfig>> GetOpenSides () {
            var states = new Dictionary<Direction, List<IRoomConfig>> () {
                {Direction.North, new List<IRoomConfig>() },
                {Direction.South, new List<IRoomConfig>() },
                {Direction.East, new List<IRoomConfig>() },
                {Direction.West, new List<IRoomConfig>() }
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
    }
}
