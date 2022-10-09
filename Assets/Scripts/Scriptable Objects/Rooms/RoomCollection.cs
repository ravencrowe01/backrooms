using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu (fileName = "Room Collection", menuName = "Backrooms/Rooms/Room Collection", order = 2)]
public class RoomCollection : ScriptableObject {
    public RoomMetadata[] Rooms;

    public RoomMetadata GetRandomRoom() {
        var roll = Random.Range (0, Rooms.Length);

        return Rooms[roll];
    }

    public RoomMetadata GetRandomRoomWithOpenSide(CardinalDirection direction) {
        var collection = Rooms.Where (r => r.IsSideOpen (direction)).ToList();

        return collection[Random.Range (0, collection.Count ())];
    }
}
