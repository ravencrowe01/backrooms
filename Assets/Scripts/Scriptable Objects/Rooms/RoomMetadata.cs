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

    public Room GetRandomRoomWithOpenSide (Direction side) => side switch {
        Direction.North => GetRandomRoomWithOpenSide (NorthRooms, side),
        Direction.South => GetRandomRoomWithOpenSide (SouthRooms, side),
        Direction.East => GetRandomRoomWithOpenSide (EastRooms, side),
        Direction.West => GetRandomRoomWithOpenSide (WestRooms, side),
        _ => throw new Exception ($"Cardinal direction {side} out of range."),
    };

    private Room GetRandomRoomWithOpenSide (RoomWeight[] sidedRooms, Direction side) {
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

    public bool IsSideOpen (Direction side) => side switch {
        Direction.North => Room.NorthOpen,
        Direction.South => Room.SouthOpen,
        Direction.East => Room.EastOpen,
        Direction.West => Room.WestOpen,
        _ => throw new Exception ($"Cardinal direction {side} out of range."),
    };

    public bool CanConnectToSide (Direction side) => side switch {
        Direction.North => Room.SouthOpen,
        Direction.South => Room.NorthOpen,
        Direction.East => Room.WestOpen,
        Direction.West => Room.EastOpen,
        _ => throw new Exception ($"Cardinal direction {side} out of range."),
    };
}