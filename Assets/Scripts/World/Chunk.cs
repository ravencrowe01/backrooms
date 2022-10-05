using UnityEngine;

public class Chunk : MonoBehaviour {
    private readonly Room[,] _rooms = new Room[3, 3];

    public void AddRoom (int x, int y, Room room) => _rooms[x, y] = room;

    public Room GetRoom (int x, int y) => _rooms[x, y];

    public void InstantiateRooms () {
        for (int x = 0; x < 3; x++) {
            for (int y = 0; y < 3; y++) {
                var room = _rooms[x, y];

                if (room != null) {
                    Instantiate (room, transform.GetChild(x).GetChild(y));
                }
            }
        }
    }

    public bool IsNorthOpen () => _rooms[0, 1].NorthOpen;

    public bool IsNorthNorthWestOpen () => _rooms[0, 0].NorthOpen;

    public bool IsWestNorthWestOpen () => _rooms[0, 0].WestOpen;

    public bool IsWestOpen () => _rooms[1, 0].WestOpen;

    public bool IsWestSouthWestOpen () => _rooms[2, 0].WestOpen;

    public bool IsSouthSouthWestOpen () => _rooms[2, 0].SouthOpen;

    public bool IsSouthOpen () => _rooms[2, 1].SouthOpen;

    public bool IsSouthSouthEastOpen () => _rooms[2, 2].SouthOpen;

    public bool IsEastSouthEastOpen () => _rooms[2, 2].EastOpen;

    public bool IsEastOpen () => _rooms[1, 2].EastOpen;

    public bool IsEastNorthEastOpen () => _rooms[0, 2].EastOpen;

    public bool IsNorthNorthEastOpen () => _rooms[0, 2].NorthOpen;
}