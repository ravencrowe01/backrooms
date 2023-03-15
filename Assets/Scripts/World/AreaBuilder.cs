using Backrooms.Assets.Scripts.RNG;
using Backrooms.Assets.Scripts.World.Config;
using Backrooms.Assets.Scripts.World.Prototypes;
using System;
using System.Drawing;
using System.Linq;
using UnityEngine;

namespace Backrooms.Assets.Scripts.World {
    public class AreaBuilder : IAreaBuilder {
        private int _width = 3;
        private int _height = 3;

        private int _cWidth = 3;
        private int _cHeight = 3;

        private int _roomSize = 1;

        public IAreaBuilder WithDiminsions (int width, int height) {
            _width = width;
            _height = height;
            return this;
        }

        public IAreaBuilder WithChunkDiminsions (int width, int height) {
            _cWidth = width;
            _cHeight = height;
            return this;
        }

        public IAreaBuilder WithRoomSize (int size) {
            _roomSize = size;
            return this;
        }

        public IAreaConfig BuildArea (IRNG rng) {
            var area = new ProtoArea (_width);

            AddSeedChunks (rng, area);

            AddChunks (rng, area);

            FixChunkConnections (area);

            return area.ToAreaConfig();
        }

        private void AddSeedChunks (IRNG rng, ProtoArea area) {
            var a = _width * _height;

            for (int i = 0; i < a / 10 + 1; i++) {
                int x, y;

                do {
                    x = rng.Next (_height);
                    y = rng.Next (_width);
                } while (area.GetChunk (x, y) is not null);

                var chunk = BuildChunk (rng, area, new Vector2 (x, y));

                area.AddChunk (chunk, x, y);
            }
        }

        private void AddChunks (IRNG rng, ProtoArea area) {
            for (int x = 0; x < _width; x++) {
                for (int y = 0; y < _height; y++) {
                    if (area.GetChunk (x, y) is null) {
                        var chunk = BuildChunk (rng, area, new Vector2 (x, y));
                        area.AddChunk (chunk, x, y);
                    }
                }
            }
        }

        private ProtoChunk BuildChunk (IRNG rng, ProtoArea area, Vector2 cords) {
            var builder = new ChunkBuilder ().WithDiminsions (_cWidth, _cHeight).WithCoordinates (cords).WithRoomSize(_roomSize);

            foreach (var dir in (Direction[]) Enum.GetValues (typeof (Direction))) {
                var nCords = cords + Utility.GetVectorFromDirection (dir);

                if (nCords.x >= 0 && nCords.x < _height && nCords.y >= 0 && nCords.y < _height) {
                    var neighbor = area.GetChunk ((int) nCords.x, (int) nCords.y);

                    if (neighbor is not null) {
                        AddConnections (builder, dir, neighbor);

                        AddHallways (rng, builder, dir, neighbor);
                    }
                }
            }

            return builder.BuildChunkAsPrototype (rng);
        }

        private void AddConnections (IChunkBuilder builder, Direction dir, ProtoChunk neighbor) {
            var opDir = Utility.GetOppositeDirection (dir);

            var open = neighbor.GetOpenSides ()[opDir].Where(r => r.GetSideState(opDir).Open);

            foreach (var op in open) {
                var target = GetConnectionTarget (op.Coordinates, dir);

                builder.WithConnection (target, dir);
            }
        }

        private Vector2 GetConnectionTarget(Vector2 origin, Direction dir) {
            if(dir == Direction.North) {
                return new Vector2 (origin.x, _cWidth - 1);
            }

            if(dir == Direction.South) {
                return new Vector2 (origin.x, 0);
            }

            if(dir == Direction.East) {
                return new Vector2 (_cWidth - 1, origin.y);
            }

            if(dir == Direction.West) {
                return new Vector2 (0, origin.y);
            }

            return new Vector2 (-1, -1);
        }

        private static void AddHallways (IRNG rng, IChunkBuilder builder, Direction dir, ProtoChunk neighbor) {
            foreach (var vec in neighbor.Hallways.Keys) {
                var hallway = neighbor.Hallways[vec];
                var roll = rng.Next (1);
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

        private void FixChunkConnections(ProtoArea area) {
            for(int x = 0; x < _width; x++) { 
                for (int y = 0; y < _width; y++) {
                    var chunk = area.GetChunk (x, y);

                    foreach(var dir in (Direction[]) Enum.GetValues (typeof (Direction))) {
                        var adjVec = Utility.GetVectorFromDirection (dir) + new Vector2 (x, y);

                        if (adjVec.x >= 0 && adjVec.x < _width && adjVec.y >= 0 && adjVec.y < _width) {
                            var adjChunk = area.GetChunk ((int) adjVec.x, (int) adjVec.y);

                            for(int i = 0; i < _cWidth; i++) {
                                Vector2 roomVec;
                                Vector2 adjRoomVec;

                                if(dir == Direction.North) {
                                    roomVec = new Vector2 (i, _cWidth - 1);
                                    adjRoomVec = new Vector2 (i, 0);
                                }
                                else if(dir == Direction.South) {
                                    roomVec = new Vector2 (i, 0);
                                    adjRoomVec = new Vector2 (i, _cWidth - 1);
                                }
                                else if (dir == Direction.East) {
                                    roomVec = new Vector2 (0, i);
                                    adjRoomVec = new Vector2 (_cWidth - 1, i);
                                }
                                else {
                                    roomVec = new Vector2 (_cWidth - 1, i);
                                    adjRoomVec = new Vector2 (0, i);
                                }

                                var room = chunk.GetRoom (roomVec);
                                var adjRoom = adjChunk.GetRoom (adjRoomVec);

                                if(room.GetSideState(dir).Open != adjRoom.GetSideState(Utility.GetOppositeDirection(dir)).Open) {
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
