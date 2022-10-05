using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour {
    public Chunk BaseChunk;
    public RoomCollection Rooms;

    public Chunk GenerateChunk (Chunk[] neighbors = null) {
        var chunk = new Chunk ();
        var rooms = new RoomMetadata[3, 3];

        rooms[1, 1] = Rooms.GetRandomRoom ();

        var startRoom = rooms[1, 1];

        var startSide = startRoom.Room.GetRandomOpenSide ();
        var startSideCords = GetStartSideCords (startSide);

        return chunk;
    }

    private Vector2 GetStartSideCords (CardinalDirection direction) => direction switch {
        CardinalDirection.North => new Vector2 (0, 1),
        CardinalDirection.South => new Vector2 (2, 1),
        CardinalDirection.East => new Vector2 (1, 2),
        CardinalDirection.West => new Vector2 (1, 0),
        _ => new Vector2 (),
    };
}
