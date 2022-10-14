using Backrooms.Scripts;
using Backrooms.Scripts.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkTest : MonoBehaviour {
    public RoomMetadata Room;
    private Room _currentRoom;
    // Start is called before the first frame update
    void Start () {
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown (KeyCode.K)) {
            if(_currentRoom != null) {
                Destroy (_currentRoom.gameObject);
                _currentRoom = null;
            }

            var room = Room.GetRandomRoomWithOpenSide (Direction.North);

            _currentRoom = Instantiate (room, transform);
        }
    }
}
