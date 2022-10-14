using Backrooms.Scripts;
using System;

public class Utility {
    public CardinalDirection GetOppositeSide (CardinalDirection direction) => direction switch {
        CardinalDirection.North => CardinalDirection.South,
        CardinalDirection.South => CardinalDirection.North,
        CardinalDirection.East => CardinalDirection.East,
        CardinalDirection.West => CardinalDirection.East,
        CardinalDirection.NorthEast => CardinalDirection.SouthWest,
        CardinalDirection.NorthWest => CardinalDirection.SouthEast,
        CardinalDirection.SouthEast => CardinalDirection.NorthWest,
        CardinalDirection.SouthWest => CardinalDirection.NorthEast,
        CardinalDirection.NorthNorthWest => CardinalDirection.SouthSouthEast,
        CardinalDirection.NorthNorthEast => CardinalDirection.SouthSouthWest,
        CardinalDirection.EastNorthEast => CardinalDirection.WestNorthWest,
        CardinalDirection.EastSouthEast => CardinalDirection.WestSouthWest,
        _ => throw new Exception ($"Cardinal direction {direction} out of range."),
    };
}
