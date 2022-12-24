using Backrooms.Assets.Scripts.Pathfinding;
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

        public RoomConfig[,] Rooms { get; private set; }

        public Dictionary<Direction, float> HallwayBuildChances { get; private set; } = new Dictionary<Direction, float> {
            {Direction.NorthNorthEast, 0 },
            {Direction.North, 0 },
            {Direction.NorthNorthWest, 0 },
            {Direction.EastNorthEast, 0 },
            {Direction.East, 0 },
            {Direction.EastSouthEast, 0 }
        };

        private ChunkRoom[,] _rooms;

        private readonly IAStar _pathfinder;

        private readonly int _width;
        private readonly int _height;

        private List<Direction> _connections = new List<Direction> ();

        private List<Direction> _hallwayConnections = new List<Direction> ();

        private bool _built = false;

        public ChunkBuilder (int width = 3, int height = 3) {
            _width = width;
            _height = height;
        }

        public void AddConnections (IEnumerable<Direction> connections) => _connections.AddRange (connections);

        public void AddHallwayConnections (IDictionary<Direction, float> connections) {
            foreach (var dir in connections.Keys) {
                var roll = Random.Range (0f, 1f);

                if(roll <= connections[dir] && !_hallwayConnections.Contains(Utility.GetOppositeSide(dir))) {
                    _hallwayConnections.Add (dir);

                    var d = HallwayBuildChances.Keys.Contains (dir) ? dir : Utility.GetOppositeSide (dir);

                    HallwayBuildChances[d] = connections[dir];
                }
            }
        }

        #region Room building
        public void BuildRooms () {
            if(!_built) {
                do {
                    ConstructRooms ();

                    AddChunkConnections ();

                    BuildHallways ();

                    FixRoomConnections ();
                }
                while (!ValidateChunk ());

                _built = true;
            }
        }

        #region Room Generation
        private void ConstructRooms () {
            _rooms = new ChunkRoom[_width, _height];

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
        #endregion

        #region Room Connection Fixing
        /// <summary>
        /// Fix connections between rooms.
        /// </summary>
        private void FixRoomConnections () {
            for (int x = 0; x <= 2; x++) {
                for (int y = 0; y <= 2; y++) {
                    var room = _rooms[x, y];

                    var adjacent = GetAdjacentRooms (x, y);

                    foreach (var key in adjacent.Keys) {
                        var direction = GetAdjacentDirection (new Vector2 (x, y), key);

                        var oppDir = Utility.GetOppositeSide (direction);

                        if (room.OpenSides.Contains (direction) != adjacent[key].OpenSides.Contains (oppDir)) {
                            var roll = Random.Range (0, 2);

                            room.SetSideState (direction, roll == 1);

                            _rooms[(int) key.x, (int) key.y].SetSideState (oppDir, roll == 1);
                        }
                    }
                }
            }
        }

        private Dictionary<Vector2, ChunkRoom> GetAdjacentRooms (int x, int y) {
            var adj = new Dictionary<Vector2, ChunkRoom> ();

            if (x + 1 < _width) {
                adj.Add (new Vector2 (x + 1, y), _rooms[x + 1, y]);
            }

            if (x - 1 >= 0) {
                adj.Add (new Vector2 (x - 1, y), _rooms[x - 1, y]);
            }

            if (y + 1 < _height) {
                adj.Add (new Vector2 (x, y + 1), _rooms[x, y + 1]);
            }

            if (y - 1 >= 0) {
                adj.Add (new Vector2 (x, y - 1), _rooms[x, y - 1]);
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

        private void AddChunkConnections () {
            var temp = new List<Direction> (_connections);

            temp.AddRange (_hallwayConnections);

            foreach (var con in temp.Distinct()) {
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
                        _rooms[0, 2].SetSideState (Direction.South, true);
                        break;
                    case Direction.South:
                        _rooms[1, 2].SetSideState (Direction.South, true);
                        break;
                    case Direction.SouthSouthWest:
                        _rooms[2, 2].SetSideState (Direction.South, true);
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
                        _rooms[2, 0].SetSideState (Direction.West, true);
                        break;
                    case Direction.West:
                        _rooms[2, 1].SetSideState (Direction.West, true);
                        break;
                    case Direction.WestNorthWest:
                        _rooms[2, 2].SetSideState (Direction.West, true);
                        break;
                        #endregion
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
         * Otherwise, roll for a connecting hallway.
         * 
         * If no older chunk has a hallway, hallway build chance is 10%.
         */
        private void BuildHallways () => _hallwayConnections.ForEach (d => BuildHallway (d, Utility.GetOppositeSide (d)));

        private void BuildHallway (Direction start, Direction end) {
            var startCords = _directionToRoomMap[start];
            var endCords = _directionToRoomMap[end];

            switch (Utility.GetOppositeSide(start)) {
                case Direction.North:
                case Direction.South:
                    BuildNorthSouthHallway (startCords, endCords);
                    break;
                case Direction.East:
                case Direction.West:
                    BuildEastWestHallway (startCords, endCords);
                    break;
            }
        }

        private void BuildNorthSouthHallway (Vector2 start, Vector2 end) {
            _rooms[(int) start.x, (int) start.y].SetSideState (Direction.North, true);
            _rooms[(int) start.x, (int) start.y].SetSideState (Direction.South, true);

            _rooms[(int) start.x, (int) (start.y + 1)].SetSideState (Direction.North, true);
            _rooms[(int) start.x, (int) (start.y + 1)].SetSideState (Direction.South, true);

            _rooms[(int) end.x, (int) end.y].SetSideState (Direction.North, true);
            _rooms[(int) end.x, (int) end.y].SetSideState (Direction.South, true);
        }

        private void BuildEastWestHallway (Vector2 start, Vector2 end) {
            _rooms[(int) start.x, (int) start.y].SetSideState (Direction.East, true);
            _rooms[(int) start.x, (int) start.y].SetSideState (Direction.West, true);

            _rooms[(int) (start.x + 1), (int) start.y].SetSideState (Direction.East, true);
            _rooms[(int) (start.x + 1), (int) start.y].SetSideState (Direction.West, true);

            _rooms[(int) end.x, (int) end.y].SetSideState (Direction.East, true);
            _rooms[(int) end.x, (int) end.y].SetSideState (Direction.West, true);
        }
        #endregion

        private bool ValidateChunk () {
            Node[,] nodeMap = BuildNodeMap ();

            for (int x = 0; x < _width; x++) {
                for (int y = 0; y < _height; y++) {
                    if (!(x == 1 && y == 1)) {
                        var pathfinder = new AStar (nodeMap[x, y], nodeMap[1, 1], nodeMap);
                        pathfinder.CheckEntrances = true;

                        PathfindingStatus pathfinding;

                        do {
                            pathfinding = pathfinder.Step ();
                        }
                        while (pathfinding == PathfindingStatus.Finding);

                        if (pathfinding == PathfindingStatus.Invalid) {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private Node[,] BuildNodeMap () {
            var nodeMap = new Node[3, 3];

            for (int x = 0; x < _width; x++) {
                for (int y = 0; y < _height; y++) {
                    nodeMap[x, y] = new Node {
                        Cost = 1,
                        Position = new Vector2 (x, y),
                        OpenEntrances = _rooms[x, y].OpenSides,
                        Blocking = false
                    };
                }
            }

            return nodeMap;
        }

        private class ChunkRoom {
            public bool NorthOpen { get; private set; }
            public bool SouthOpen { get; private set; }
            public bool EastOpen { get; private set; }
            public bool WestOpen { get; private set; }

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