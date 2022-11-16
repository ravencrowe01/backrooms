using Backrooms.Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Backrooms.Assets.Scripts.World {
    public class ChunkBuilder {
        private readonly Dictionary<Direction, Vector2> _directionToRoomMap = new Dictionary<Direction, Vector2> () {
                {Direction.North, new Vector2 (1, 0) },
                {Direction.NorthEast, new Vector2 (0, 0) },
                {Direction.NorthNorthEast, new Vector2 (0, 0) },
                {Direction.EastNorthEast, new Vector2 (0, 0) },
                {Direction.East, new Vector2 (0, 1) },
                {Direction.SouthEast, new Vector2 (0, 2) },
                {Direction.SouthSouthEast, new Vector2 (0, 2) },
                {Direction.EastSouthEast, new Vector2 (0, 2) },
                {Direction.South, new Vector2 (1, 2) },
                {Direction.SouthWest, new Vector2 (2, 2) },
                {Direction.SouthSouthWest, new Vector2 (2, 2) },
                {Direction.WestSouthWest, new Vector2 (2, 2) },
                {Direction.West, new Vector2 (2, 1) },
                {Direction.NorthWest, new Vector2 (2, 0) },
                {Direction.WestNorthWest, new Vector2 (2, 0) },
                {Direction.NorthNorthWest, new Vector2 (2, 0) }

            };

        private readonly int _width;
        private readonly int _height;

        public ChunkRoom[,] Rooms { get; private set; }

        public ChunkBuilder (int width = 3, int height = 3) {
            _width = width;
            _height = height;
        }

        #region Room building
        public void BuildRooms (IEnumerable<Direction> connections) {
            ConstructRooms ();

            AddChunkConnections (connections);

            // This is hard and I'll do it later
            FixRoomConnections ();
        }

        private void ConstructRooms () {
            Rooms = new ChunkRoom[_width, _height];

            for (int x = 0; x <= 2; x++) {
                for (int y = 0; y <= 2; y++) {
                    Rooms[x, y] = ConstructRoom (x, y);
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

            var openCount = GetRandomOpenCount (x, y, directions.Count, 0);

            AddOpenSides (room, directions, openCount);

            return room;
        }

        private void AddOpenSides (ChunkRoom room, IList<Direction> directions, int amount) {
            while (amount > 0) {
                var chosen = directions[Random.Range (0, directions.Count)];

                room.SetSideState (chosen, true);

                directions.Remove (chosen);

                amount--;
            }
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

        private int GetRandomOpenCount (int x, int y, int max, int min = 1) {
            if (x == 1 && y == 1) {
                return Random.Range (min, Math.Min (3, max));
            }

            return Random.Range (min, Math.Min (4, max));
        }

        private void FixRoomConnections () {
            // Connect every room to the center room

        }

        private void AddChunkConnections (IEnumerable<Direction> connections) {
            foreach (var con in connections) {
                switch (con) {
                    #region North
                    case Direction.NorthNorthEast:
                        Rooms[0, 0].SetSideState (Direction.North, true);
                        break;
                    case Direction.North:
                        Rooms[1, 0].SetSideState (Direction.North, true);
                        break;
                    case Direction.NorthNorthWest:
                        Rooms[2, 0].SetSideState (Direction.North, true);
                        break;
                    #endregion

                    #region South
                    case Direction.SouthSouthEast:
                        Rooms[0, 2].SetSideState (Direction.South, true);
                        break;
                    case Direction.South:
                        Rooms[1, 2].SetSideState (Direction.South, true);
                        break;
                    case Direction.SouthSouthWest:
                        Rooms[2, 2].SetSideState (Direction.South, true);
                        break;
                    #endregion

                    #region East
                    case Direction.EastNorthEast:
                        Rooms[0, 0].SetSideState (Direction.East, true);
                        break;
                    case Direction.East:
                        Rooms[0, 1].SetSideState (Direction.East, true);
                        break;
                    case Direction.EastSouthEast:
                        Rooms[0, 2].SetSideState (Direction.East, true);
                        break;
                    #endregion

                    #region West
                    case Direction.WestSouthWest:
                        Rooms[2, 0].SetSideState (Direction.West, true);
                        break;
                    case Direction.West:
                        Rooms[2, 1].SetSideState (Direction.West, true);
                        break;
                    case Direction.WestNorthWest:
                        Rooms[2, 2].SetSideState (Direction.West, true);
                        break;
                        #endregion
                }
            }
        }
        #endregion

        #region Hallway building
        /// When a new chunk is built, if there is an older chunk with a hallway connecting to the new chunk,
        /// check the older chunk's connecting hallway chance. 
        /// 
        /// If the chance is below 10%, don't build a hallway.
        /// 
        /// Otherwise, roll for a connecting hallway.
        /// 
        /// If no older chunk has a hallway, hallway build chance is 10%.

        public void AddHallway (Direction start, Direction end) {
            var startCords = _directionToRoomMap[start];
            var endCords = _directionToRoomMap[end];

            var buildDirection = GetHallwayBuildDirection (startCords, endCords);

            switch (buildDirection) {
                case Direction.South:
                    BuildNorthSouthHallway (startCords, endCords);
                    break;
                case Direction.East:
                    BuildEastWestHallway (startCords, endCords);
                    break;
            }
        }

        private Direction GetHallwayBuildDirection (Vector2 start, Vector2 end) {
            if (start.y != end.y) {
                return Direction.South;
            }

            if (start.x != end.x) {
                return Direction.East;
            }

            throw new ArgumentException ("Hallways must connect from one edge of a chunk to another in a straight line.");
        }

        private void BuildNorthSouthHallway (Vector2 start, Vector2 end) {
            Rooms[(int) start.x, (int) start.y].SetSideState (Direction.South, true);

            Rooms[(int) start.x, (int) (start.y + 1)].SetSideState (Direction.North, true);
            Rooms[(int) start.x, (int) (start.y + 1)].SetSideState (Direction.South, true);

            Rooms[(int) end.x, (int) end.y].SetSideState (Direction.North, true);
        }

        private void BuildEastWestHallway (Vector2 start, Vector2 end) {
            Rooms[(int) start.x, (int) start.y].SetSideState (Direction.East, true);

            Rooms[(int) (start.x + 1), (int) start.y].SetSideState (Direction.East, true);
            Rooms[(int) (start.x + 1), (int) start.y].SetSideState (Direction.West, true);

            Rooms[(int) end.x, (int) end.y].SetSideState (Direction.West, true);
        }
        #endregion

        public class ChunkRoom {
            public bool NorthOpen { get; private set; }
            public bool SouthOpen { get; private set; }
            public bool EastOpen { get; private set; }
            public bool WestOpen { get; private set; }

            public IEnumerable<Direction> GetOpenSides () {
                var sides = new List<Direction> ();

                if (NorthOpen) {
                    sides.Add (Direction.North);
                }

                if (SouthOpen) {
                    sides.Add (Direction.South);
                }

                if (EastOpen) {
                    sides.Add (Direction.East);
                }

                if (WestOpen) {
                    sides.Add (Direction.West);
                }

                return sides;
            }

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
    }
}