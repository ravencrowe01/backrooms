using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChunkGenerator : MonoBehaviour {
    public Chunk BaseChunk;
    public RoomCollection Rooms;

    private void Update () {
        if (Input.GetKeyDown (KeyCode.L)) {
            GenerateChunk ();
        }
    }

    public Chunk GenerateChunk (Chunk[] neighbors = null) {
        //var chunk = Instantiate (BaseChunk);
        var rooms = new RoomMetadata[3, 3];
        var openSides = new ChunkRowSides[] {
            new ChunkRowSides(),
            new ChunkRowSides(),
            new ChunkRowSides()
        };

        for (int x = 0; x <= 2; x++) {
            for (int y = 0; y <= 2; y++) {
                openSides[x].RoomSides[y] = ConstructRoom (x, y);
            }
        }

        // TODO Continue generating chunk

        return null;
    }

    private ChunkRoomSides ConstructRoom (int x, int y) {
        var dirs = new List<CardinalDirection> { CardinalDirection.North, CardinalDirection.South, CardinalDirection.East, CardinalDirection.West };
        var openCount = GetRandomOpenCount (x, y);
        var room = new ChunkRoomSides ();

        if (x == 1 && y == 1) {
            AddStartSides (room, dirs, dirs);
            AddStartSides (room, dirs, dirs);
            openCount -= 2;
        }
        else {
            var minOpen = GetMinmumOpenSides (x, y);
            AddStartSides (room, dirs, minOpen);
            openCount -= 1;
        }

        while (openCount > 0) {
            var chosen = dirs[Random.Range (0, dirs.Count)];

            dirs.Remove (chosen);

            room.Open.Add (chosen);

            openCount--;
        }

        return room;
    }

    private static void AddStartSides (ChunkRoomSides room, List<CardinalDirection> dirs, IList<CardinalDirection> minOpen) {
        var chosen = minOpen[Random.Range (0, minOpen.Count)];

        room.Open.Add (chosen);

        dirs.Remove (chosen);
    }

    private IList<CardinalDirection> GetMinmumOpenSides (int x, int y) {
        var sides = new List<CardinalDirection> ();

        switch (x) {
            case 0:
                sides.Add (CardinalDirection.East);
                break;
            case 1:
                sides.Add (CardinalDirection.East);
                sides.Add (CardinalDirection.West);
                break;
            case 2:
                sides.Add (CardinalDirection.West);
                break;
        }

        switch (y) {
            case 0:
                sides.Add (CardinalDirection.South);
                break;
            case 1:
                sides.Add (CardinalDirection.South);
                sides.Add (CardinalDirection.North);
                break;
            case 2:
                sides.Add (CardinalDirection.North);
                break;
        }

        return sides;
    }

    private int GetRandomOpenCount (int x, int y) {
        if (x == 1 && y == 1) {
            return Random.Range (1, 3);
        }

        return Random.Range (1, 4);
    }

    private CardinalDirection GetOppositeSide (CardinalDirection direction) => direction switch {
        CardinalDirection.North => CardinalDirection.South,
        CardinalDirection.South => CardinalDirection.North,
        CardinalDirection.East => CardinalDirection.East,
        CardinalDirection.West => CardinalDirection.East,
        _ => throw new Exception ($"Cardinal direction {direction} out of range."),
    };

    private class ChunkRowSides {
        public ChunkRoomSides[] RoomSides { get; set; }

        public ChunkRowSides () {
            RoomSides = new ChunkRoomSides[] {
                new ChunkRoomSides(),
                new ChunkRoomSides(),
                new ChunkRoomSides()
            };
        }
    }

    private class ChunkRoomSides {
        public List<CardinalDirection> Open { get; set; } = new List<CardinalDirection> ();
    }
}
