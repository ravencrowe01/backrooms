using System;

namespace Backrooms.Assets.Scripts {
    public class Utility {
        public static Direction GetOppositeSide (Direction direction) => direction switch {
            Direction.North => Direction.South,
            Direction.South => Direction.North,
            Direction.East => Direction.West,
            Direction.West => Direction.East,
            Direction.NorthEast => Direction.SouthWest,
            Direction.NorthWest => Direction.SouthEast,
            Direction.SouthEast => Direction.NorthWest,
            Direction.SouthWest => Direction.NorthEast,
            Direction.NorthNorthWest => Direction.SouthSouthEast,
            Direction.NorthNorthEast => Direction.SouthSouthWest,
            Direction.EastNorthEast => Direction.WestNorthWest,
            Direction.EastSouthEast => Direction.WestSouthWest,
            _ => throw new Exception ($"Cardinal direction {direction} out of range."),
        };
    }
}