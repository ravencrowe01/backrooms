using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu (fileName = "Room Metadata", menuName = "Backrooms/Rooms/Room Metadata", order = 1)]
public class RoomMetadata : ScriptableObject {
    [Serializable]
    public struct RoomWeight {
        public Room Room;
        public int Weight;
    }

    public Room Room;
    public int Weight;

    public RoomWeight[] CommonRooms;
    public int CommonRoomsWeightTotal {
        get {
            int weight = 0;

            foreach (var room in CommonRooms) {
                weight += room.Weight;
            }

            return weight;
        }
    }

    public RoomWeight[] NorthRooms;
    public int NorthRoomsWeightTotal {
        get {
            int weight = CommonRoomsWeightTotal;

            foreach (var room in NorthRooms) {
                weight += room.Weight;
            }

            return weight;
        }
    }

    public RoomWeight[] SouthRooms;
    public int SouthRoomsWeightTotal {
        get {
            int weight = CommonRoomsWeightTotal;

            foreach (var room in SouthRooms) {
                weight += room.Weight;
            }

            return weight;
        }
    }

    public RoomWeight[] EastRooms;
    public int EastRoomsWeightTotal {
        get {
            int weight = CommonRoomsWeightTotal;

            foreach (var room in EastRooms) {
                weight += room.Weight;
            }

            return weight;
        }
    }

    public RoomWeight[] WestRooms;
    public int WestRoomsWeightTotal {
        get {
            int weight = CommonRoomsWeightTotal;

            foreach (var room in WestRooms) {
                weight += room.Weight;
            }

            return weight;
        }
    }

    public Room DefaultRandomRoom;

    public Room GetRandomRoomWithOpenSide (CardinalDirection side) => side switch {
        CardinalDirection.North => GetRandomRoomWithOpenSide (NorthRooms, side),
        CardinalDirection.South => GetRandomRoomWithOpenSide (SouthRooms, side),
        CardinalDirection.East => GetRandomRoomWithOpenSide (EastRooms, side),
        CardinalDirection.West => GetRandomRoomWithOpenSide (WestRooms, side),
        _ => throw new Exception ($"Cardinal direction {side} out of range."),
    };

    private Room GetRandomRoomWithOpenSide (RoomWeight[] sidedRooms, CardinalDirection side) {
        var rooms = GetAvailableRooms (sidedRooms);
        var tries = 0;

        do {
            var roll = Random.Range (0, NorthRoomsWeightTotal);

            var total = 0;

            foreach (var r in rooms) {
                total += r.Weight;

                if (roll <= total && IsSideOpen(r.Room, side)) {
                    return r.Room;
                }
            }

            tries++;
        }
        while (tries <= 5);

        return DefaultRandomRoom;
    }

    private RoomWeight[] GetAvailableRooms (RoomWeight[] sidedRooms) {
        var rooms = new RoomWeight[CommonRooms.Length + sidedRooms.Length];

        CommonRooms.CopyTo (rooms, 0);
        sidedRooms.CopyTo (rooms, CommonRooms.Length + 1);

        return rooms;
    }

    private bool IsSideOpen (Room room, CardinalDirection side) => side switch {
        CardinalDirection.North => room.NorthOpen,
        CardinalDirection.South => room.SouthOpen,
        CardinalDirection.East => room.EastOpen,
        CardinalDirection.West => room.WestOpen,
        _ => throw new Exception ($"Cardinal direction {side} out of range."),
    };
}