using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Backrooms.Scripts.World {
    public class ChunkBuilder : MonoBehaviour {
        public class ChunkRoom {
            public bool NorthOpen { get; private set; }
            public bool SouthOpen { get; private set; }
            public bool EastOpen { get; private set; }
            public bool WestOpen { get; private set; }

            public void SetSideState (Direction direction, bool open) {
                switch (direction) {
                    case Direction.North:
                        NorthOpen = open;
                        break;
                    case Direction.South:
                        SouthOpen = open;
                        break;
                    case Direction.East:
                        EastOpen = open;
                        break;
                    case Direction.West:
                        WestOpen = open;
                        break;
                }
            }
        }

        private readonly int _width;
        private readonly int _height;

        private ChunkRoom[,] _rooms;

        public ChunkBuilder (int width = 3, int height = 3) {
            _width = width;
            _height = height;
        }

        #region Room building
        public void BuildRooms (Direction[] connections) {
            ConstructRooms ();

            // TODO Continue generating chunk

            // This is hard and I'll do it later
            FixRoomConnections ();

            AddChunkConnections (connections);
        }

        private void ConstructRooms () {
            var _rooms = new ChunkRoom[_width, _height];

            for (int x = 0; x <= 2; x++) {
                for (int y = 0; y <= 2; y++) {
                    _rooms[x, y] = ConstructRoom (x, y);
                }
            }
        }

        private ChunkRoom ConstructRoom (int x, int y) {
            var directions = new List<Direction> { Direction.North, Direction.South, Direction.East, Direction.West };
            var room = new ChunkRoom ();

            //The center room needs to have at least two open sides.
            if (x == 1 && y == 1) {
                AddMinimumSides (room, directions, directions, 2);
            }
            // The outer rooms need to have at least one open side.
            else {
                var minOpen = GetMinmumOpenSides (x, y);
                AddMinimumSides (room, directions, minOpen, 1);
            }

            var openCount = GetRandomOpenCount (x, y);

            AddOpenSides (room, directions, openCount);

            return room;
        }

        private void AddOpenSides (ChunkRoom room, IList<Direction> directions, int amount) {
            do {
                var chosen = directions[Random.Range (0, directions.Count)];

                room.SetSideState (chosen, true);

                directions.Remove (chosen);

                amount--;
            } while (amount > 0);
        }

        private void AddMinimumSides (ChunkRoom room, IList<Direction> directions, IList<Direction> minimum, int amount) {
            var temp = new List<Direction> (minimum);

            AddOpenSides (room, minimum, amount);

            temp.AddRange (minimum);

            var open = temp.Distinct ().ToList ();

            open.ForEach (d => directions.Remove (d));
        }

        private IList<Direction> GetMinmumOpenSides (int x, int y) {
            var sides = new List<Direction> ();

            switch (x) {
                case 0:
                    sides.Add (Direction.East);
                    break;
                case 1:
                    sides.AddRange (new List<Direction> { Direction.East, Direction.West });
                    break;
                case 2:
                    sides.Add (Direction.West);
                    break;
            }

            switch (y) {
                case 2:
                    sides.Add (Direction.North);
                    break;
                case 1:
                    sides.AddRange (new List<Direction> { Direction.North, Direction.South });
                    break;
                case 0:
                    sides.Add (Direction.South);
                    break;
            }

            return sides;
        }

        private int GetRandomOpenCount (int x, int y) {
            if (x == 1 && y == 1) {
                return Random.Range (1, 3);
            }

            return Random.Range (1, 4);
        }

        private void FixRoomConnections () {

        }

        private void AddChunkConnections (Direction[] connections) {
            foreach (var con in connections) {
                switch (con) {
                    #region North
                    case Direction.NorthNorthEast:
                        _rooms[0, 0].SetSideState (Direction.North, true);
                        break;
                    case Direction.North:
                        _rooms[1, 0].SetSideState (Direction.North, true);
                        break;
                    case Direction.NorthNorthWest:
                        _rooms[2, 0].SetSideState (Direction.North, true);
                        break;
                    #endregion

                    #region South
                    case Direction.SouthSouthEast:
                        _rooms[0, 0].SetSideState (Direction.South, true);
                        break;
                    case Direction.South:
                        _rooms[1, 0].SetSideState (Direction.South, true);
                        break;
                    case Direction.SouthSouthWest:
                        _rooms[2, 0].SetSideState (Direction.South, true);
                        break;
                    #endregion

                    #region East
                    case Direction.EastNorthEast:
                        _rooms[0, 0].SetSideState (Direction.East, true);
                        break;
                    case Direction.East:
                        _rooms[0, 1].SetSideState (Direction.East, true);
                        break;
                    case Direction.EastSouthEast:
                        _rooms[0, 2].SetSideState (Direction.East, true);
                        break;
                    #endregion

                    #region West
                    case Direction.WestSouthWest:
                        _rooms[0, 0].SetSideState (Direction.West, true);
                        break;
                    case Direction.West:
                        _rooms[0, 1].SetSideState (Direction.West, true);
                        break;
                    case Direction.WestNorthWest:
                        _rooms[0, 2].SetSideState (Direction.West, true);
                        break;
                        #endregion
                }
            }
        }
        #endregion

        #region Hallway building
        public void AddHallway (Direction start, Direction end) {
            var startCords = GetOuterRoomCordsFromDirection (start);
            var endCords = GetOuterRoomCordsFromDirection (end);

            var buildDirection = GetHallwayBuildDirection (startCords, endCords);

            switch(buildDirection) {
                case Direction.South:
                    BuildNorthSouthHallway (startCords, endCords);
                    break;
                case Direction.East:
                    BuildEastWestHallway (startCords, endCords);
                    break;
            }
        }

        private Vector2 GetOuterRoomCordsFromDirection(Direction direction) {
            return direction switch {
                Direction.North => new Vector2(1, 0),
                Direction.NorthEast or Direction.NorthNorthEast or Direction.EastNorthEast => new Vector2(0, 0),
                Direction.East => new Vector2(0, 1),
                Direction.SouthEast or Direction.EastSouthEast or Direction.SouthSouthEast => new Vector2(0, 2),
                Direction.South => new Vector2(1, 2),
                Direction.SouthWest or Direction.SouthSouthWest or Direction.WestSouthWest => new Vector2(2, 2),
                Direction.West => new Vector2(2, 1),
                Direction.NorthWest or Direction.WestNorthWest or Direction.NorthNorthWest => new Vector2(2, 0),
                _ => throw new ArgumentException ($"Unknow cardinal direction {direction}"),
            };
        }

        private Direction GetHallwayBuildDirection(Vector2 start, Vector2 end) {
            if(start.y != end.y) {
                return Direction.South;
            }

            if (start.x != end.x) {
                return Direction.East;
            }

            throw new ArgumentException ("Hallways must connect from one edge of a chunk to another in a straight line.");
        }

        private void BuildNorthSouthHallway(Vector2 start, Vector2 end) {
            _rooms[(int) start.x, (int) start.y].SetSideState (Direction.South, true);

            _rooms[(int) start.x, (int) (start.y + 1)].SetSideState (Direction.North, true);
            _rooms[(int) start.x, (int) (start.y + 1)].SetSideState (Direction.South, true);

            _rooms[(int) end.x, (int) end.y].SetSideState (Direction.North, true);
        }

        private void BuildEastWestHallway (Vector2 start, Vector2 end) {
            _rooms[(int) start.x, (int) start.y].SetSideState (Direction.East, true);

            _rooms[(int) (start.x + 1), (int) start.y].SetSideState (Direction.East, true);
            _rooms[(int) (start.x + 1), (int) start.y].SetSideState (Direction.West, true);

            _rooms[(int) end.x, (int) end.y].SetSideState (Direction.West, true);
        }
        #endregion
    }
}