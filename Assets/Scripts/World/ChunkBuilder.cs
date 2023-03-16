using Backrooms.Assets.Scripts.World.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using Backrooms.Assets.Scripts.World.Prototypes;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.UI;

namespace Backrooms.Assets.Scripts.World {
    public class ChunkBuilder : IChunkBuilder {
        private int _width = 3;
        private int _height = 3;

        private int _roomSize = 1;

        private Dictionary<Vector2, Dictionary<Direction, ProtoSideState>> _chunkConnections = new ();

        private Dictionary<Vector2, ProtoHallway> _hallways = new ();

        private Vector2 _cords;


        #region Fluent methods
        public IChunkBuilder WithDiminsions (int width, int height) {
            _width = width;
            _height = height;
            return this;
        }

        public IChunkBuilder WithRoomSize (int amt = 1) {
            _roomSize = amt;
            return this;
        }

        public IChunkBuilder WithConnection (Vector2 connectedRoom, Direction dir) {
            var side = new ProtoSideState (_roomSize);

            side.SetTotalState (true);

            if (!_chunkConnections.ContainsKey (connectedRoom)) {
                _chunkConnections.Add (connectedRoom, BuildConnectionDict(dir, side));
            }
            else {
                _chunkConnections[connectedRoom][dir].SetTotalState(true);
            }

            return this;

            Dictionary<Direction, ProtoSideState> BuildConnectionDict(Direction dir, ProtoSideState side) {
                var dict = new Dictionary<Direction, ProtoSideState> {
                    { Direction.North, new ProtoSideState(_roomSize) },
                    { Direction.South, new ProtoSideState(_roomSize) },
                    { Direction.East, new ProtoSideState(_roomSize) },
                    { Direction.West, new ProtoSideState(_roomSize) },
                };

                dict[dir] = side;

                return dict;
            }
        }

        public IChunkBuilder WithHallway (Vector2 start, float chance, Direction dir) {
            if (IsEdgeRoom (start)) {
                if (start.x != 0 && start.y == _height - 1) {
                    start.y = 0;
                }

                if (start.y != 0 && start.x == _width - 1) {
                    start.x = 0;
                }


                if (!_hallways.ContainsKey (start)) {
                    if (dir == Direction.North) {
                        dir = Direction.South;
                    }

                    if (dir == Direction.West) {
                        dir = Direction.East;
                    }

                    _hallways.Add (start, new ProtoHallway (chance, dir));
                }
            }
            else {
                // TODO InvalidArgumentException
                throw new Exception ($"[{GetType ().Name}.{nameof (WithHallway)}]: Was not given an edge room for hallway building");
            }

            return this;

            bool IsEdgeRoom (Vector2 start) => start.x == 0 || start.x == _width - 1 || start.y == 0 || start.y == _height - 1;
        }

        public IChunkBuilder WithCoordinates (Vector2 cords) {
            _cords = cords;
            return this;
        }

        public ProtoChunk BuildChunkAsPrototype (int seed) {
            var chunk = new ProtoChunk (_width);

            do {
                ConstructRooms (chunk, seed);

                AddChunkConnections (chunk);

                BuildHallways (chunk);

                FixRoomConnections (chunk, seed);
            } while (!ChunkValidator.ValidateChunk (chunk));

            return chunk;
        }

        public IChunkConfig BuildChunk (int seed) => BuildChunkAsPrototype (seed).ToChunkConfig (_cords);
        #endregion

        #region Room Construction
        private void ConstructRooms (ProtoChunk chunk, int seed) {
            for (int x = 0; x < _width; x++) {
                for (int y = 0; y < _height; y++) {
                    chunk.AddRoom (new Vector2 (x, y), ConstructRoom (x, y, seed));
                }
            }
        }

        private ProtoRoom ConstructRoom (int x, int y, int seed) {
            var room = new ProtoRoom (new Vector2 (x, y), _roomSize);
            var openable = GetOpenableSides (x, y);

            // The outer rooms need to have at least one open side.
            AddOpenSides (room, openable, seed);

            // The center room needs to have at least two open sides.
            if (x == 0 && y == 0 && room.GetOpenSides ().Count < 2) {
                AddOpenSides (room, openable, seed);
            }

            return room;
        }

        private IList<Direction> GetOpenableSides (int x, int y) {
            var sides = new List<Direction> ();

            // TODO RoomOutOfBoundsException
            if (x < 0 || x > _width || y < 0 || y > _height) {
                throw new IndexOutOfRangeException ($"[{GetType ().Name}.{nameof (GetOpenableSides)}]: Tried to access out of bounds room {{{x}, {y}}}");
            }

            if (x == 0) {
                sides.Add (Direction.East);
            }
            else if (x == _width - 1) {
                sides.Add (Direction.West);
            }
            else {
                sides.AddRange (new List<Direction> { Direction.East, Direction.West });
            }

            if (y == 0) {
                sides.Add (Direction.North);
            }
            else if (y == _height - 1) {
                sides.Add (Direction.South);
            }
            else {
                sides.AddRange (new List<Direction> { Direction.North, Direction.South });
            }

            return sides;
        }

        private void AddOpenSides (ProtoRoom room, IList<Direction> directions, int seed) {
            Random.InitState (seed ^ (int) room.Coordinates.x ^ (int) room.Coordinates.y);
            var amt = Random.Range (1, directions.Count + 1);

            while (amt > 0) {
                var chosenDir = directions[Random.Range (0, directions.Count)];

                var chosenSide = room.GetSideState (chosenDir);

                var states = Random.Range (1, _roomSize + 1);

                var tries = 0;

                do {
                    var state = Random.Range (0, _roomSize);

                    if (!chosenSide.GetState (state)) {
                        chosenSide.SetState (state, true);
                        states--;
                    }

                    tries++;

                } while (states > 0 && tries < 100);

                directions.Remove (chosenDir);

                amt--;
            }
        }
        #endregion

        #region Chunk Connecting
        private void AddChunkConnections (ProtoChunk chunk) {
            if (_chunkConnections.Count == 0) {
                CreateRandomConnections ();
            }

            foreach (var cord in _chunkConnections.Keys) {
                foreach (var side in _chunkConnections[cord]) {
                    for (int i = 0; i < _roomSize; i++) {
                        chunk.SetRoomSideState (cord, side.Key, i, true);
                    }
                }
            }
        }

        private void CreateRandomConnections () {
            var roll = Random.Range (2, 9);

            while (roll > 0) {
                var dir = (Direction) Random.Range (0, 4);

                var room = Random.Range (0, _width);

                Vector2 roomCords = GetConnectionRoomCoordinates (dir, room);

                var side = new ProtoSideState (_roomSize);

                side.SetTotalState (true);

                if (_chunkConnections.ContainsKey (roomCords)) {
                    if (!_chunkConnections[roomCords].ContainsKey (dir)) {
                        _chunkConnections[roomCords].Add (dir, side);

                        roll--;
                    }
                }
                else {
                    _chunkConnections.Add (roomCords, new Dictionary<Direction, ProtoSideState> () {
                        { dir, side }
                    });

                    roll--;
                }
            }
        }

        private Vector2 GetConnectionRoomCoordinates (Direction dir, int room) {
            if (dir == Direction.North) {
                return new Vector2 (room, _width - 1);
            }

            else if (dir == Direction.South) {
                return new Vector2 (room, 0);
            }

            else if (dir == Direction.East) {
                return new Vector2 (_width - 1, room);
            }

            else if (dir == Direction.West) {
                return new Vector2 (0, room);
            }

            return new Vector2(-1, -1);
        }
        #endregion

        #region Hallway building
        /* When a new chunk is built, if there is an older chunk with a hallway connecting to the new chunk,
         * check the older chunk's connecting hallway chance. 
         * 
         * If the chance is below 10%, don't build a hallway.
         * 
         * Otherwise, roll for a connecting hallway against older chunk chance * 0.6.
         * 
         * If no older chunk has a hallway, hallway build chance is 10%.
         */

        private void BuildHallways (ProtoChunk chunk) {
            foreach (var cord in _hallways.Keys) {
                var hallway = _hallways[cord];

                BuildHallway (chunk, cord, hallway.Direction);
            }
        }

        private void BuildHallway (ProtoChunk chunk, Vector2 start, Direction dir) {
            var limit = dir == Direction.South ? _height : _width;

            for (int i = 0; i < limit; i++) {
                var x = dir == Direction.South ? start.x : i;
                var y = dir == Direction.East ? start.y : i;

                var vec = new Vector2 (x, y);

                if (i >= 0 && y < limit - 1) {
                    OpenRoom (vec, dir);
                }

                if (i <= limit - 1 && y > 0) {
                    OpenRoom (vec, Utility.GetOppositeDirection (dir));
                }
            }

            void OpenRoom (Vector2 vec, Direction dir) {
                for (int i = 0; i < _roomSize; i++) {
                    chunk.SetRoomSideState (vec, dir, i, true);
                }
            }
        }
        #endregion

        #region Room Connection Fixing
        /// <summary>
        /// Fix connections between rooms.
        /// </summary>
        private void FixRoomConnections (ProtoChunk chunk, int seed) {
            for (int x = 0; x < _width; x++) {
                for (int y = 0; y < _height; y++) {
                    Random.InitState (seed ^ x ^ y);

                    var room = new Vector2 (x, y);

                    var adjacent = Utility.GetAdjacentVectors (room, _width, _height);

                    foreach (var neighbor in adjacent) {
                        var dir = Utility.GetDirectionFromVector (neighbor - room);

                        var neighborDir = Utility.GetDirectionFromVector (room - neighbor);

                        for (int i = 0; i < _roomSize; i++) {
                            if (chunk.GetRoom (room).GetSideState (dir, i) != chunk.GetRoom (neighbor).GetSideState (neighborDir, i)) {
                                var roll = Random.Range (0, 2) == 1;

                                chunk.SetRoomSideState (room, dir, i, roll);

                                chunk.SetRoomSideState (neighbor, neighborDir, i, roll);
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
