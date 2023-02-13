using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Backrooms.Assets.Scripts.World.Config {
    public class ChunkConfig : IChunkConfig {
        public Vector2 Coordinates => new Vector2 (_cords.x, _cords.y);
        private Vector2 _cords;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public IRoomConfig[,] Rooms => (IRoomConfig[,]) _rooms.Clone ();
        private IRoomConfig[,] _rooms;

        public IReadOnlyList<IHallwayConfig> Hallways => _hallways;
        private List<IHallwayConfig> _hallways;

        public ChunkConfig (Vector2 cords, IRoomConfig[,] rooms, List<IHallwayConfig> hallways) {
            _cords = cords;
            Width = rooms.GetLength (0);
            Height = rooms.GetLength (1);
            _rooms = rooms;
            _hallways = hallways;
        }

        public ChunkConfig (Vector2 cords, int width, int height) {
            _cords = cords;
            Width = width;
            Height = height;
            _rooms = new IRoomConfig[width, height];
            _hallways = new List<IHallwayConfig> ();
        }

        public void SetRoom (int x, int y, IRoomConfig room) => _rooms[x, y] = room;

        public void AddHallway (Vector2 origin, Direction dir, float chance) => _hallways.Add (new HallwayConfig (origin, dir, chance));

        public Dictionary<Direction, List<IRoomConfig>> GetOpenSides () {
            var states = new Dictionary<Direction, List<IRoomConfig>> ();

            for (int x = 0; x < Width; x++) {
                for (int y = 0; y < Height; y++) {
                    if (IsEdgeRoom (x, y)) {
                        var open = _rooms[x, y].GetOpenSides ();

                        if (x != 0) {
                            open.Remove (Direction.West);
                        }

                        if (x != Width - 1) {
                            open.Remove (Direction.East);
                        }

                        if (y != 0) {
                            open.Remove (Direction.North);
                        }

                        if (y != Height - 1) {
                            open.Remove (Direction.South);
                        }

                        foreach (var o in open) {
                            states[o].Add (_rooms[x, y]);
                        }
                    }
                }
            }

            return states;

            bool IsEdgeRoom (int x, int y) => x == 0 && y >= 0 || x >= 0 && y == 0 || x == Width - 1 && y >= 0 || x >= 0 && y == Height - 1;
        }
    }
}
