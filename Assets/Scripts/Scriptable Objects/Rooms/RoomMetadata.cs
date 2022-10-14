using Backrooms.Scripts;
using Backrooms.Scripts.World;
using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu (fileName = "Room Metadata", menuName = "Backrooms/Rooms/Room Metadata", order = 1)]
public class RoomMetadata : ScriptableObject {
    [Serializable]
    public struct RoomWeight {
        public RoomMetadata RoomMetadata;
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
        var rooms = GetAvailableRooms (sidedRooms).Where (r => r.RoomMetadata.IsSideOpen (side));

        var roll = Random.Range (0, NorthRoomsWeightTotal);

        var total = 0;

        foreach (var r in rooms) {
            total += r.Weight;

            if (roll <= total && r.RoomMetadata.IsSideOpen (side)) {
                return r.RoomMetadata.Room;
            }
        }

        return DefaultRandomRoom;
    }

    private RoomWeight[] GetAvailableRooms (RoomWeight[] sidedRooms) {
        var rooms = new RoomWeight[CommonRooms.Length + sidedRooms.Length];

        CommonRooms.CopyTo (rooms, 0);
        sidedRooms.CopyTo (rooms, CommonRooms.Length);

        return rooms;
    }

    public bool IsSideOpen (CardinalDirection side) => side switch {
        CardinalDirection.North => Room.NorthOpen,
        CardinalDirection.South => Room.SouthOpen,
        CardinalDirection.East => Room.EastOpen,
        CardinalDirection.West => Room.WestOpen,
        _ => throw new Exception ($"Cardinal direction {side} out of range."),
    };

    public bool CanConnectToSide (CardinalDirection side) => side switch {
        CardinalDirection.North => Room.SouthOpen,
        CardinalDirection.South => Room.NorthOpen,
        CardinalDirection.East => Room.WestOpen,
        CardinalDirection.West => Room.EastOpen,
        _ => throw new Exception ($"Cardinal direction {side} out of range."),
    };
}