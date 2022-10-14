using UnityEngine;

namespace Backrooms.Scripts.World {
    public class Chunk : MonoBehaviour {
        private readonly Room[,] _rooms = new Room[3, 3];

        #region North
        public bool NorthNorthEastOpen => _rooms[0, 0].NorthOpen;
        public bool NorthOpen => _rooms[1, 0].NorthOpen;
        public bool NorthNorthWestOpen => _rooms[2, 0].NorthOpen;
        #endregion

        #region South
        public bool SouthSouthWestOpen => _rooms[0, 2].SouthOpen;
        public bool SouthOpen => _rooms[1, 2].SouthOpen;
        public bool SouthSouthEastOpen => _rooms[2, 2].SouthOpen;
        #endregion

        #region East
        public bool EastSouthEastOpen => _rooms[0, 0].EastOpen;
        public bool EastOpen => _rooms[0, 1].EastOpen;
        public bool EastNorthEastOpen => _rooms[0, 2].EastOpen;
        #endregion

        #region West
        public bool WestNorthWestOpen => _rooms[0, 0].WestOpen;
        public bool WestOpen => _rooms[2, 1].WestOpen;
        public bool WestSouthWestOpen => _rooms[0, 2].WestOpen;
        #endregion

        private void Start () {
            for (int x = 0; x < 3; x++) {
                for (int y = 0; y < 3; y++) {
                    _rooms[x, y] = new Room ();
                }
            }
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