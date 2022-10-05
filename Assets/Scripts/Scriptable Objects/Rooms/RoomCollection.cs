using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Room Collection", menuName = "Backrooms/Rooms/Room Collection", order = 2)]
public class RoomCollection : ScriptableObject {
    public RoomMetadata[] Rooms;

    public RoomMetadata GetRandomRoom() {
        var roll = Random.Range (0, Rooms.Length);

        return Rooms[roll];
    }
}
