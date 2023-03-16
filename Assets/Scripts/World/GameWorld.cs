using Backrooms.Assets.Scripts.World.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Backrooms.Assets.Scripts.World {
    public class GameWorld {
        public string Name => _config.Name;

        public int Seed => _config.Seed;

        public Vector3 PlayerPosition => _config.Player.transform.position;

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

            var area = builder.BuildArea (Seed);

            for (int x = 0; x < area.Chunks.GetLength (0); x++) {
                for (int z = 0; z < area.Chunks.GetLength (1); z++) {
                    FinalizeChunk (Seed, area.Chunks[x, z], x, z);
                }
            }
        }

        // TODO Need a ChunkBuilder registry
        // TODO Check if a special chunk needs to spawn
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

                    AddHallways (Seed, builder, dir, neighbor);
                }
            }

            FinalizeChunk (Seed, builder.BuildChunk (Seed), x, z);
        }

        private void FinalizeChunk (int seed, IChunkConfig chunk, int x, int z) {
            _config.ChunkRoot.AddChunk (chunk, _config.ChunkBase, seed);

            _chunks.Add (_config.ChunkRoot.FindChunk (x, z));
        }

        private void AddChunkConnections (IChunkBuilder builder, Direction dir, Chunk neighbor) {
            var opDir = Utility.GetOppositeDirection (dir);

            var open = neighbor.OpenConnections[opDir];

            foreach (var op in open) {
                var roomCords = op.Coordinates - Utility.GetVectorFromDirection (opDir);

                for (int i = 0; i < _config.RoomSize; i++) {
                    builder.WithConnection (roomCords, dir);
                }
            }
        }

        private static void AddHallways (int seed, IChunkBuilder builder, Direction dir, Chunk neighbor) {
            foreach (var hallway in neighbor.Hallways) {
                Random.InitState (seed ^ (int) neighbor.Coordinates.x ^ (int) neighbor.Coordinates.y);
                var roll = Random.Range (0, 101) / 100f;
                var chance = hallway.Chance * 0.6f;

                if (roll <= chance) {
                    switch (hallway.Direction) {
                        case Direction.North when dir == Direction.North || dir == Direction.South:
                            builder.WithHallway (hallway.Origin, chance, Direction.South);
                            break;
                        case Direction.East when dir == Direction.East || dir == Direction.West:
                            builder.WithHallway (hallway.Origin, chance, Direction.West);
                            break;
                    }
                }
            }
        }
    }
}
