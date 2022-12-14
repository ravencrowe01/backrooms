using Backrooms.Assets.Scripts.Pathfinding;
using System;
using System.Collections.Generic;
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

        private readonly Vector2Int _dimensions;

        private readonly Vector2 _center;

        public ChunkRoom[,] Rooms { get; private set; }

        public ChunkBuilder (Vector2Int dimensions, Vector2 center) {
            _dimensions = dimensions;
            _center = center;
        }

        #region Room building
        public void BuildRooms (IEnumerable<Direction> connections) {
            do {
                ConstructRooms ();

                AddChunkConnections (connections);

                FixRoomConnections ();
            }
            while (!IsChunkValid ());
        }

        #region Room Generation
        private void ConstructRooms () {
            Rooms = new ChunkRoom[_dimensions.x, _dimensions.y];

            for (int x = 0; x <= 2; x++) {
                for (int y = 0; y <= 2; y++) {
                    Rooms[x, y] = ConstructRoom (x, y);
                }
            }
        }

        private ChunkRoom ConstructRoom (int x, int y) {
            var room = new ChunkRoom ();

            var available = GetOpenableSides (x, y);

            var open = ChooseOpenSides (available, (x == 1 && y == 1 ? 2 : 1));

            AddOpenSides (room, open);

            return room;
        }

        private IList<Direction> GetOpenableSides (int x, int y) {
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

        private IList<Direction> ChooseOpenSides (IList<Direction> available, int amount = 1) {
            var sides = new List<Direction> ();

            for (; amount > 0; amount--) {
                var roll = Random.Range (0, available.Count);

                sides.Add (available[roll]);

                available.RemoveAt (roll);
            }

            return sides;
        }

        private void AddOpenSides (ChunkRoom room, IList<Direction> open) {
            foreach (var side in open) {
                room.SetSideState (side, true);
            }
        }
        #endregion

        #region Room Connection Fixing
        /// <summary>
        /// Fix connections between rooms.
        /// </summary>
        private void FixRoomConnections () {
            for (int x = 0; x <= 2; x++) {
                for (int y = 0; y <= 2; y++) {
                    var room = Rooms[x, y];

                    var adjacent = GetAdjacentRooms (x, y);

                    foreach (var key in adjacent.Keys) {
                        var direction = GetAdjacentDirection (new Vector2 (x, y), key);

                        var oppDir = Utility.GetOppositeSide (direction);

                        if (room.OpenSides.Contains (direction) != adjacent[key].OpenSides.Contains (oppDir)) {
                            var roll = Random.Range (0, 2);

                            room.SetSideState (direction, roll == 1);

                            Rooms[(int) key.x, (int) key.y].SetSideState (oppDir, roll == 1);
                        }
                    }
                }
            }
        }

        private Dictionary<Vector2, ChunkRoom> GetAdjacentRooms (int x, int y) {
            var adj = new Dictionary<Vector2, ChunkRoom> ();

            if (x + 1 < _dimensions.x) {
                adj.Add (new Vector2 (x + 1, y), Rooms[x + 1, y]);
            }

            if (x - 1 >= 0) {
                adj.Add (new Vector2 (x - 1, y), Rooms[x - 1, y]);
            }

            if (y + 1 < _dimensions.y) {
                adj.Add (new Vector2 (x, y + 1), Rooms[x, y + 1]);
            }

            if (y - 1 >= 0) {
                adj.Add (new Vector2 (x, y - 1), Rooms[x, y - 1]);
            }

            return adj;
        }

        private Direction GetAdjacentDirection (Vector2 origin, Vector2 target) {
            var dirVec = target - origin;

            dirVec = new Vector2 (Mathf.Clamp (dirVec.x, -1, 1), Mathf.Clamp (dirVec.y, -1, 1));

            if (dirVec == Vector2.down) {
                return Direction.North;
            }

            if (dirVec == Vector2.up) {
                return Direction.South;
            }

            if (dirVec == Vector2.right) {
                return Direction.East;
            }

            return Direction.West;
        }
        #endregion

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

        private bool IsChunkValid () {
            Node[,] nodeMap = BuildNodeMap ();

            for (int x = 0; x < _dimensions.x; x++) {
                for (int y = 0; y < _dimensions.y; y++) {
                    if (!(x == 1 && y == 1)) {
                        if (!ValidateChunk (nodeMap, x, y)) {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private bool ValidateChunk (Node[,] nodeMap, int x, int y) {
            var pathfinder = new AStar (nodeMap[x, y], nodeMap[(int) _center.x, (int) _center.y], nodeMap) {
                CheckEntrances = true
            };

            PathfindingStatus pathfinding;

            do {
                pathfinding = pathfinder.Step ();
            }
            while (pathfinding == PathfindingStatus.Finding);

            if (pathfinding == PathfindingStatus.Invalid) {
                return false;
            }

            return true;
        }

        private Node[,] BuildNodeMap () {
            var nodeMap = new Node[3, 3];

            for (int x = 0; x < _dimensions.x; x++) {
                for (int y = 0; y < _dimensions.y; y++) {
                    nodeMap[x, y] = new Node {
                        Cost = 1,
                        Position = new Vector2 (x, y),
                        OpenEntrances = Rooms[x, y].OpenSides,
                        Blocking = false
                    };
                }
            }

            return nodeMap;
        }

        public class ChunkRoom {
            public bool NorthOpen { get; /*private*/ set; }
            public bool SouthOpen { get; /*private*/ set; }
            public bool EastOpen { get; /*private*/ set; }
            public bool WestOpen { get; /*private*/ set; }

            public List<Direction> OpenSides => GetOpenSides ();

            private List<Direction> GetOpenSides () {
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