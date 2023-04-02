using Backrooms.Assets.Scripts.World.Config;
using Backrooms.Assets.Scripts.World.Prototypes;
using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Backrooms.Assets.Scripts.World {
    public class AreaBuilder : IAreaBuilder {
        private int _size = 3;

        private int _chunkSize = 3;

        private int _roomSize = 1;

        public IAreaBuilder WithSize (int size) {
            _size = size;
            return this;
        }

        public IAreaBuilder WithChunkSize (int size) {
            _chunkSize = size;
            return this;
        }

        public IAreaBuilder WithRoomSize (int size) {
            _roomSize = size;
            return this;
        }

        public IAreaConfig BuildArea (int seed) {
            var area = new ProtoArea (_size);

            AddSeedChunks (seed, area);

            AddChunks (seed, area);

            FixChunkConnections (area);

            return area.ToAreaConfig ();
        }

        private void AddSeedChunks (int seed, ProtoArea area) {
            var a = _size * _size;
            Random.InitState (seed);

            for (int i = 0; i < a / 10 + 1; i++) {
                int x, y;

                do {
                    x = Random.Range (0, _size);
                    y = Random.Range (0, _size);
                } while (area.GetChunk (x, y) is not null);

                var chunk = BuildChunk (seed, area, new Vector2 (x, y));

                area.AddChunk (chunk, x, y);
            }
        }

        private void AddChunks (int seed, ProtoArea area) {
            for (int x = 0; x < _size; x++) {
                for (int y = 0; y < _size; y++) {
                    if (area.GetChunk (x, y) is null) {
                        var chunk = BuildChunk (seed, area, new Vector2 (x, y));
                        area.AddChunk (chunk, x, y);
                    }
                }
            }
        }

        private ProtoChunk BuildChunk (int seed, ProtoArea area, Vector2 cords) {
            var builder = new ChunkBuilder ().WithDiminsions (_chunkSize).WithCoordinates (cords).WithRoomSize (_roomSize);

            foreach (var dir in (Direction[]) Enum.GetValues (typeof (Direction))) {
                var nCords = cords + Utility.GetVectorFromDirection (dir);

                if (nCords.x >= 0 && nCords.x < _size && nCords.y >= 0 && nCords.y < _size) {
                    var neighbor = area.GetChunk ((int) nCords.x, (int) nCords.y);

                    if (neighbor is not null) {
                        AddConnections (builder, dir, neighbor);

                        AddHallways (seed, builder, dir, neighbor);
                    }
                }
            }

            return builder.BuildChunkAsPrototype (seed);
        }

        private void AddConnections (IChunkBuilder builder, Direction dir, ProtoChunk neighbor) {
            var opDir = Utility.GetOppositeDirection (dir);

            var open = neighbor.GetOpenSides ()[opDir].Where (r => r.GetSideState (opDir).Open);

            foreach (var op in open) {
                var target = GetConnectionTarget (op.Coordinates, dir);

                builder.WithConnection (target, dir);
            }
        }

        private Vector2 GetConnectionTarget (Vector2 origin, Direction dir) {
            if (dir == Direction.North) {
                return new Vector2 (origin.x, _chunkSize - 1);
            }

            if (dir == Direction.South) {
                return new Vector2 (origin.x, 0);
            }

            if (dir == Direction.East) {
                return new Vector2 (_chunkSize - 1, origin.y);
            }

            if (dir == Direction.West) {
                return new Vector2 (0, origin.y);
            }

            return new Vector2 (-1, -1);
        }

        private static void AddHallways (int seed, IChunkBuilder builder, Direction dir, ProtoChunk neighbor) {
            foreach (var vec in neighbor.Hallways.Keys) {
                var hallway = neighbor.Hallways[vec];

                Random.InitState (seed ^ (int) vec.x ^ (int) vec.y);
                var roll = Random.Range (0, 101) / 100f;

                var chance = hallway.BuildChance * 0.6f;

                if (roll <= chance) {
                    switch (hallway.Direction) {
                        case Direction.North when dir == Direction.North || dir == Direction.South:
                            builder.WithHallway (vec, chance, Direction.South);
                            break;
                        case Direction.East when dir == Direction.East || dir == Direction.West:
                            builder.WithHallway (vec, chance, Direction.West);
                            break;
                    }
                }
            }
        }

        private void FixChunkConnections (ProtoArea area) {
            for (int x = 0; x < _size; x++) {
                for (int y = 0; y < _size; y++) {
                    var chunk = area.GetChunk (x, y);

                    foreach (var dir in (Direction[]) Enum.GetValues (typeof (Direction))) {
                        var adjVec = Utility.GetVectorFromDirection (dir) + new Vector2 (x, y);

                        if (adjVec.x >= 0 && adjVec.x < _size && adjVec.y >= 0 && adjVec.y < _size) {
                            var adjChunk = area.GetChunk ((int) adjVec.x, (int) adjVec.y);

                            for (int i = 0; i < _chunkSize; i++) {
                                Vector2 roomVec;
                                Vector2 adjRoomVec;

                                if (dir == Direction.North) {
                                    roomVec = new Vector2 (i, _chunkSize - 1);
                                    adjRoomVec = new Vector2 (i, 0);
                                }
                                else if (dir == Direction.South) {
                                    roomVec = new Vector2 (i, 0);
                                    adjRoomVec = new Vector2 (i, _chunkSize - 1);
                                }
                                else if (dir == Direction.East) {
                                    roomVec = new Vector2 (0, i);
                                    adjRoomVec = new Vector2 (_chunkSize - 1, i);
                                }
                                else {
                                    roomVec = new Vector2 (_chunkSize - 1, i);
                                    adjRoomVec = new Vector2 (0, i);
                                }

                                var room = chunk.GetRoom (roomVec);
                                var adjRoom = adjChunk.GetRoom (adjRoomVec);

                                if (room.GetSideState (dir).Open != adjRoom.GetSideState (Utility.GetOppositeDirection (dir)).Open) {
                                    var open = DateTime.Now.Ticks & 1;

                                    room.SetSideTotalState (dir, false);
                                    adjRoom.SetSideTotalState (Utility.GetOppositeDirection (dir), false);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
