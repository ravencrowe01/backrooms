using Backrooms.Assets.Scripts.RNG;
using Backrooms.Assets.Scripts.World.Config;
using System;
using UnityEngine;

namespace Backrooms.Assets.Scripts.World {
    public class AreaBuilder : IAreaBuilder {
        private int _width = 3;
        private int _height = 3;

        private int _cWidth = 3;
        private int _cHeight = 3;

        private int _roomSize = 1;

        private IRNG _rng;

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

        public AreaConfig BuildArea (IRNG rng) {
            var area = new AreaConfig (_width, _height);

            AddSeedChunks (rng, area);

            AddChunks (rng, area);

            return area;
        }

        private void AddSeedChunks (IRNG rng, AreaConfig area) {

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

        private void AddChunks (IRNG rng, AreaConfig area) {
            for (int x = 0; x < _width; x++) {
                for (int y = 0; y < _height; y++) {
                    if (area.GetChunk (x, y) is null) {
                        var chunk = BuildChunk (rng, area, new Vector2 (x, y));
                        area.AddChunk (chunk, x, y);
                    }
                }
            }
        }

        private IChunkConfig BuildChunk (IRNG rng, AreaConfig area, Vector2 cords) {
            var builder = new ChunkBuilder ().WithDiminsions (_cWidth, _cHeight).WithCoordinates (cords).WithRoomSize(_roomSize);

            foreach (var dir in (Direction[]) Enum.GetValues (typeof (Direction))) {
                var nCords = cords + Utility.GetVectorFromDirection (dir);

                var neighbor = area.GetChunk ((int) nCords.x, (int) nCords.y);

                if (neighbor is not null) {
                    AddConnections (builder, dir, neighbor);

                    AddHallways (rng, builder, dir, neighbor);
                }
            }

            return builder.BuildChunk (rng);
        }

        private void AddConnections (IChunkBuilder builder, Direction dir, IChunkConfig neighbor) {
            var opDir = Utility.GetOppositeDirection (dir);

            var open = neighbor.GetOpenSides ()[opDir];

            foreach (var op in open) {
                var roomCords = op.Coordinates - Utility.GetVectorFromDirection (opDir);

                for (int i = 0; i < _roomSize; i++) {
                    builder.WithConnection (roomCords, dir, i);
                }
            }
        }

        private static void AddHallways (IRNG rng, IChunkBuilder builder, Direction dir, IChunkConfig neighbor) {
            foreach (var hallway in neighbor.Hallways) {
                var roll = rng.Next (1);
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
