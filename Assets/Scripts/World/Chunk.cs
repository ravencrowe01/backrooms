using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Backrooms.Scripts.World {
    public class Chunk : MonoBehaviour {
        public int Height = 3;
        public int Width = 3;

        private Room[,] _rooms;

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

        public Room GetRoom (int x, int y) => _rooms[x, y];

        public void InstantiateRooms () {
            for (int x = 0; x < 3; x++) {
                for (int y = 0; y < 3; y++) {
                    var room = _rooms[x, y];

                    if (room != null) {
                        Instantiate (room, transform.GetChild (x).GetChild (y));
                    }
                }
            }
        }
    }
}