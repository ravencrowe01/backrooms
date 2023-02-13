using Backrooms.Assets.Scripts.RNG;
using Backrooms.Assets.Scripts.World.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

        public IChunkBuilder WithConnection (Vector2 connectedRoom, Direction dir, int index) {
            var side = new ProtoSideState (_roomSize);

            side.SetState (index, true);

            if (!_chunkConnections.ContainsKey (connectedRoom)) {
                _chunkConnections.Add (connectedRoom, new Dictionary<Direction, ProtoSideState> { { dir, side } });
            }
            else {
                _chunkConnections[connectedRoom][dir].SetState (index, true);
            }

            return this;
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

        public IChunkConfig BuildChunk (IRNG RNG) {
            var chunk = new ProtoChunk (_width, _height);
            ChunkConfig config;

            do {
                ConstructRooms (chunk, RNG);

                AddChunkConnections (chunk);

                BuildHallways (chunk);

                FixRoomConnections (chunk, RNG);

                config = chunk.ToChunkConfig (_cords);
            } while (!ChunkValidator.ValidateChunk (config));

            return config;
        }
        #endregion

        #region Room Construction
        private void ConstructRooms (ProtoChunk chunk, IRNG rand) {
            for (int x = 0; x < _width; x++) {
                for (int y = 0; y < _height; y++) {
                    chunk.AddRoom (new Vector2 (x, y), ConstructRoom (x, y, rand));
                }
            }
        }

        private ProtoRoom ConstructRoom (int x, int y, IRNG IRNG) {
            var room = new ProtoRoom (new Vector2 (x, y), _roomSize);
            var openable = GetOpenableSides (x, y);

            //The center room needs to have at least two open sides.
            // The outer rooms need to have at least one open side.
            AddOpenSides (room, openable, IRNG);

            if (x == 0 && y == 0 && room.GetOpenSides ().Count < 2) {
                AddOpenSides (room, openable, IRNG);
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

        private void AddOpenSides (ProtoRoom room, IList<Direction> directions, IRNG IRNG) {
            var amt = IRNG.Next (1, directions.Count + 1);

            while (amt > 0) {
                var chosenDir = directions[IRNG.Next (0, directions.Count)];

                var chosenSide = room.GetSideState (chosenDir);

                var states = IRNG.Next (1, _roomSize + 1);

                var tries = 0;

                do {
                    var state = IRNG.Next (0, _roomSize);

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
            foreach (var cord in _chunkConnections.Keys) {
                foreach (var side in _chunkConnections[cord]) {
                    for (int i = 0; i < _roomSize; i++) {
                        chunk.SetRoomSideState (cord, side.Key, i, true);
                    }
                }
            }
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
        private void FixRoomConnections (ProtoChunk chunk, IRNG rand) {
            for (int x = 0; x < _width; x++) {
                for (int y = 0; y < _height; y++) {
                    var room = new Vector2 (x, y);

                    var adjacent = Utility.GetAdjacentVectors (room, _width, _height);

                    foreach (var neighbor in adjacent) {
                        var dir = Utility.GetDirectionFromVector (neighbor - room);

                        var neighborDir = Utility.GetDirectionFromVector (room - neighbor);

                        for (int i = 0; i < _roomSize; i++) {
                            if (chunk.GetRoom (room).GetSideState (dir, i) != chunk.GetRoom (neighbor).GetSideState (neighborDir, i)) {
                                var roll = rand.Next (2) == 1;

                                chunk.SetRoomSideState (room, dir, i, roll);

                                chunk.SetRoomSideState (neighbor, neighborDir, i, roll);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        private class ProtoChunk {
            private ProtoRoom[,] _rooms;

            private Dictionary<Vector2, ProtoHallway> _hallways;

            public ProtoChunk (int width, int height) {
                _rooms = new ProtoRoom[width, height];
                _hallways = new Dictionary<Vector2, ProtoHallway> ();
            }

            public void AddRoom (Vector2 cords, ProtoRoom room) => _rooms[(int) cords.x, (int) cords.y] = room;

            public ProtoRoom GetRoom (Vector2 cords) => _rooms[(int) cords.x, (int) cords.y];

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

        private class ProtoRoom {
            private Vector2 _cords;
            private Dictionary<Direction, ProtoSideState> _states;

            public ProtoRoom (Vector2 cords, int size) {
                _cords = cords;

                _states = new Dictionary<Direction, ProtoSideState> {
                    {Direction.North, new ProtoSideState(size) },
                    {Direction.South, new ProtoSideState(size) },
                    {Direction.East, new ProtoSideState(size) },
                    {Direction.West, new ProtoSideState(size) }
                };
            }

            public Dictionary<Direction, ProtoSideState> GetOpenSides () {
                var open = new Dictionary<Direction, ProtoSideState> ();

                foreach (var dir in (Direction[]) Enum.GetValues (typeof (Direction))) {
                    if (_states[dir].Open) {
                        open.Add (dir, _states[dir]);
                    }
                }

                return open;
            }

            public void SetSideState (Direction dir, int i, bool state) => _states[dir].SetState (i, state);

            public bool GetSideState (Direction dir, int i) => _states[dir].GetState (i);

            public ProtoSideState GetSideState (Direction dir) => _states[dir];

            public IRoomConfig ToRoomConfig () {
                var states = new Dictionary<Direction, ISideStateConfig> ();

                foreach (var state in _states.Keys) {
                    states.Add (state, _states[state].ToSideStateConfig ());
                }

                return new RoomConfig (_cords, states);
            }
        }

        private class ProtoSideState {
            public bool[] _states;

            public bool Open => _states.Any (s => s);

            public ProtoSideState (int size) {
                _states = new bool[size];
            }

            public ProtoSideState (bool[] states) {
                _states = states;
            }

            public ISideStateConfig ToSideStateConfig () => new SideStateConfig (_states);

            public void SetState (int i, bool state) => _states[i] = state;

            public bool GetState (int i) => _states[i];
        }

        private struct ProtoHallway {
            public float BuildChance { get; private set; }
            public Direction Direction { get; private set; }

            public ProtoHallway (float chance, Direction direction) {
                BuildChance = chance;
                Direction = direction;
            }
        }
    }
}
