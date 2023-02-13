using Backrooms.Assets.Scripts.Config;
using Backrooms.Assets.Scripts.RNG;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Backrooms.Assets.Scripts.World {
    public class GameWorld {
        private IGameWorldConfig _config;

        private List<Chunk> _chunks;

        public GameWorld (IGameWorldConfig config) {
            _config = config;
            _chunks = new List<Chunk> ();
        }

        public void GenerateStartingArea () {
            var builder = new AreaBuilder ();

            builder.WithDiminsions (_config.StartingAreaWidth, _config.StartingAreaHeight)
                .WithChunkDiminsions (_config.ChunkWidth, _config.ChunkHeight)
                .WithRoomSize (_config.RoomSize);

            var rng = new RNGProvider ();
            rng.SetSeed (_config.Seed);

            var area = builder.BuildArea (rng);

            for (int x = 0; x < area.Chunks.GetLength (0); x++) {
                for (int z = 0; z < area.Chunks.GetLength (1); z++) {
                    _config.ChunkRoot.AddChunk (area.Chunks[x, z], _config.ChunkBase, rng);

                    _chunks.Add (_config.ChunkRoot.FindChunk (x, z));
                }
            }
        }

        public void GenerateChunk (int x, int z) {
            var builder = new ChunkBuilder ();
            var cords = new Vector2 (x, z);

            builder.WithDiminsions (_config.ChunkWidth, _config.ChunkHeight)
                .WithRoomSize (_config.RoomSize);

            foreach (var dir in Enum.GetValues (typeof (Direction)).Cast<Direction> ()) {
                var nCords = cords + Utility.GetVectorFromDirection (dir);

                var neighbor = _chunks.FirstOrDefault (c => c.Coordinates == nCords);

                if (neighbor is not null) {
                    AddChunkConnections (builder, dir, neighbor);
                }
            }
        }

        private void AddChunkConnections (IChunkBuilder builder, Direction dir, Chunk neighbor) {
            var opDir = Utility.GetOppositeDirection (dir);

            var open = neighbor.OpenConnections[opDir];

            foreach (var op in open) {
                var roomCords = op.Coordinates - Utility.GetVectorFromDirection (opDir);

                for (int i = 0; i < _config.RoomSize; i++) {
                    builder.WithConnection (roomCords, dir, 0);
                }
            }
        }
    }
}
