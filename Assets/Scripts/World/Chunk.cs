using Backrooms.Assets.Scripts;
using Backrooms.Assets.Scripts.Databases;
using System.Collections.Generic;
using UnityEngine;

namespace Backrooms.Assets.Scripts.World {
    public class Chunk : MonoBehaviour {
        public int Height = 3;
        public int Width = 3;

        private readonly List<Direction> _connections = new List<Direction> ();
        private Room[,] _rooms;

        public float ConnectingHallwayBuildChance = -1f;

        #region North Open States
        public bool NorthNorthEastOpen => _rooms[0, 0].NorthOpen;
        public bool NorthOpen => _rooms[1, 0].NorthOpen;
        public bool NorthNorthWestOpen => _rooms[2, 0].NorthOpen;
        #endregion

        #region South Open States
        public bool SouthSouthWestOpen => _rooms[0, 2].SouthOpen;
        public bool SouthOpen => _rooms[1, 2].SouthOpen;
        public bool SouthSouthEastOpen => _rooms[2, 2].SouthOpen;
        #endregion

        #region East Open States
        public bool EastSouthEastOpen => _rooms[0, 0].EastOpen;
        public bool EastOpen => _rooms[0, 1].EastOpen;
        public bool EastNorthEastOpen => _rooms[0, 2].EastOpen;
        #endregion

        #region West Open States
        public bool WestNorthWestOpen => _rooms[0, 0].WestOpen;
        public bool WestOpen => _rooms[2, 1].WestOpen;
        public bool WestSouthWestOpen => _rooms[0, 2].WestOpen;
        #endregion

        private void Awake () {
            _rooms = new Room[Width, Height];
        }

        public void AddConnections (IEnumerable<Direction> connections) => _connections.AddRange (connections);

        public void BuildChunk () {
            var chunkBuilder = new ChunkBuilder (null);

            chunkBuilder.BuildRooms (_connections);

            for (int x = 0; x < Width; x++) {
                for (int y = 0; y < Height; y++) {
                    var room = chunkBuilder.Rooms[x, y];

                    _rooms[x, y] = RoomDatabase.Instance.GetRandomRoomWithOpenSides (room.GetOpenSides ());
                }
            }
        }

        public IEnumerable<Direction> GetConnections (Direction direction) {
            switch (direction) {
                case Direction.North:
                    return GetNorthConnections ();
                case Direction.South:
                    return GetSouthConnections ();
                case Direction.East:
                    return GetEastConnections ();
                case Direction.West:
                    return GetWestConnections ();
                default:
                    return new List<Direction> ();
            }
        }

        private IEnumerable<Direction> GetNorthConnections () {
            var con = new List<Direction> ();

            if (NorthNorthEastOpen) {
                con.Add (Direction.NorthNorthEast);
            }

            if (NorthOpen) {
                con.Add (Direction.North);
            }

            if (NorthNorthWestOpen) {
                con.Add (Direction.NorthNorthWest);
            }

            return con;
        }

        private IEnumerable<Direction> GetSouthConnections () {
            var con = new List<Direction> ();

            if (SouthSouthEastOpen) {
                con.Add (Direction.SouthSouthEast);
            }

            if (SouthOpen) {
                con.Add (Direction.South);
            }

            if (SouthSouthWestOpen) {
                con.Add (Direction.SouthSouthWest);
            }

            return con;
        }

        private IEnumerable<Direction> GetEastConnections () {
            var con = new List<Direction> ();

            if (EastNorthEastOpen) {
                con.Add (Direction.EastNorthEast);
            }

            if (EastOpen) {
                con.Add (Direction.East);
            }

            if (EastSouthEastOpen) {
                con.Add (Direction.EastSouthEast);
            }

            return con;
        }

        private IEnumerable<Direction> GetWestConnections () {
            var con = new List<Direction> ();

            if (WestSouthWestOpen) {
                con.Add (Direction.WestSouthWest);
            }

            if (WestOpen) {
                con.Add (Direction.West);
            }

            if (WestNorthWestOpen) {
                con.Add (Direction.WestNorthWest);
            }

            return con;
        }

        public void AddRoom (int x, int y, Room room) {
            _rooms[x, y] = room;
        }

        public Room GetRoom (int x, int y) => _rooms[x, y];

        public void InstantiateRooms () {
            for (int x = 0; x < Width; x++) {
                for (int y = 0; y < Height; y++) {
                    var room = _rooms[x, y];

                    if (room != null) {
                        Instantiate (room, transform.GetChild (x).GetChild (y));
                    }
                }
            }
        }
    }
}