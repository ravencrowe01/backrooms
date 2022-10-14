using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChunkGenerator : MonoBehaviour {
    private class ChunkColumn {
        public ChunkRoom[] Rooms { get; set; }
    }

    private class ChunkRoom {
        public bool NorthOpen { get; private set; }
        public bool SouthOpen { get; private set; }
        public bool EastOpen { get; private set; }
        public bool WestOpen { get; private set; }

        public void SetSide (CardinalDirection direction, bool open) {
            switch (direction) {
                case CardinalDirection.North:
                    NorthOpen = open;
                    break;
                case CardinalDirection.South:
                    SouthOpen = open;
                    break;
                case CardinalDirection.East:
                    EastOpen = open;
                    break;
                case CardinalDirection.West:
                    WestOpen = open;
                    break;
            }
        }
    }

    public Chunk BaseChunk;
    public RoomCollection Rooms;

    public Chunk GenerateChunk (CardinalDirection[] connections) {
        var chunk = Instantiate (BaseChunk);
        var rooms = new RoomMetadata[3, 3];
        ChunkColumn[] openSides = ConstructRooms ();

        // TODO Continue generating chunk

        // This is hard and I'll do it later
        FixRoomConnections (openSides);

        AddChunkConnections (openSides, connections);

        return null;
    }

    private ChunkColumn[] ConstructRooms () {
        var openSides = new ChunkColumn[] {
            new ChunkColumn(),
            new ChunkColumn(),
            new ChunkColumn()
        };

        for (int x = 0; x <= 2; x++) {
            for (int y = 0; y <= 2; y++) {
                openSides[x].Rooms[y] = ConstructRoom (x, y);
            }
        }

        return openSides;
    }

    private ChunkRoom ConstructRoom (int x, int y) {
        var directions = new List<CardinalDirection> { CardinalDirection.North, CardinalDirection.South, CardinalDirection.East, CardinalDirection.West };
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

        var openCount = GetRandomOpenCount (x, y);

        AddOpenSides (room, directions, openCount);

        return room;
    }

    private void AddOpenSides (ChunkRoom room, IList<CardinalDirection> directions, int amount) {
        do {
            var chosen = directions[Random.Range (0, directions.Count)];

            room.SetSide (chosen, true);

            directions.Remove (chosen);

            amount--;
        } while (amount > 0);
    }

    private void AddMinimumSides (ChunkRoom room, IList<CardinalDirection> directions, IList<CardinalDirection> minimum, int amount) {
        var temp = new List<CardinalDirection> (minimum);

        AddOpenSides (room, minimum, amount);

        temp.AddRange (minimum);

        var open = temp.Distinct ().ToList ();

        open.ForEach (d => directions.Remove (d));
    }

    private IList<CardinalDirection> GetMinmumOpenSides (int x, int y) {
        var sides = new List<CardinalDirection> ();

        switch (x) {
            case 0:
                sides.Add (CardinalDirection.East);
                break;
            case 1:
                sides.AddRange (new List<CardinalDirection> { CardinalDirection.East, CardinalDirection.West });
                break;
            case 2:
                sides.Add (CardinalDirection.West);
                break;
        }

        switch (y) {
            case 2:
                sides.Add (CardinalDirection.North);
                break;
            case 1:
                sides.AddRange (new List<CardinalDirection> { CardinalDirection.North, CardinalDirection.South });
                break;
            case 0:
                sides.Add (CardinalDirection.South);
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

    private void FixRoomConnections (ChunkColumn[] rooms) {

    }

    private void AddChunkConnections (ChunkColumn[] rooms, CardinalDirection[] connections) {
        foreach (var con in connections) {
            switch (con) {
                #region North
                case CardinalDirection.NorthNorthEast:
                    rooms[0].Rooms[0].SetSide (CardinalDirection.North, true);
                    break;
                case CardinalDirection.North:
                    rooms[1].Rooms[0].SetSide (CardinalDirection.North, true);
                    break;
                case CardinalDirection.NorthNorthWest:
                    rooms[2].Rooms[0].SetSide (CardinalDirection.North, true);
                    break;
                #endregion

                #region South
                case CardinalDirection.SouthSouthEast:
                    rooms[0].Rooms[0].SetSide (CardinalDirection.South, true);
                    break;
                case CardinalDirection.South:
                    rooms[1].Rooms[0].SetSide (CardinalDirection.South, true);
                    break;
                case CardinalDirection.SouthSouthWest:
                    rooms[2].Rooms[0].SetSide (CardinalDirection.South, true);
                    break;
                #endregion

                #region
                case CardinalDirection.EastNorthEast:
                    rooms[0].Rooms[0].SetSide (CardinalDirection.East, true);
                    break;
                case CardinalDirection.East:
                    rooms[0].Rooms[1].SetSide (CardinalDirection.East, true);
                    break;
                case CardinalDirection.EastSouthEast:
                    rooms[0].Rooms[2].SetSide (CardinalDirection.East, true);
                    break;
                #endregion

                #region
                case CardinalDirection.WestSouthWest:
                    rooms[0].Rooms[0].SetSide (CardinalDirection.West, true);
                    break;
                case CardinalDirection.West:
                    rooms[0].Rooms[1].SetSide (CardinalDirection.West, true);
                    break;
                case CardinalDirection.WestNorthWest:
                    rooms[0].Rooms[2].SetSide (CardinalDirection.West, true);
                    break;
                    #endregion
            }
        }
    }
}
