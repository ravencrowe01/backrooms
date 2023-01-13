using System;
using System.Collections.Generic;
using UnityEngine;

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

        public static List<Direction> CardinalDirections = new List<Direction> {
            Direction.North,
            Direction.South,
            Direction.West,
            Direction.East
        };

        public static readonly Dictionary<Direction, Vector2> DirectionToRoomMap = new Dictionary<Direction, Vector2> () {
                {Direction.North, new Vector2 (1, 0) },
                {Direction.NorthEast, new Vector2 (0, 0) },
                {Direction.NorthNorthEast, new Vector2 (0, 0) },
                {Direction.EastNorthEast, new Vector2 (0, 0) },
                {Direction.East, new Vector2 (0, 1) },
                {Direction.SouthEast, new Vector2 (0, 2) },
                {Direction.SouthSouthEast, new Vector2 (0, 2) },
                {Direction.EastSouthEast, new Vector2 (0, 2) },
                {Direction.South, new Vector2 (1, 2) },
                {Direction.SouthWest, new Vector2 (2, 2) },
                {Direction.SouthSouthWest, new Vector2 (2, 2) },
                {Direction.WestSouthWest, new Vector2 (2, 2) },
                {Direction.West, new Vector2 (2, 1) },
                {Direction.NorthWest, new Vector2 (2, 0) },
                {Direction.WestNorthWest, new Vector2 (2, 0) },
                {Direction.NorthNorthWest, new Vector2 (2, 0) }
        };
    }
}