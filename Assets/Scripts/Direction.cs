namespace Backrooms.Assets.Scripts {
    public enum Direction {
        North = 1,
        South = 2,
        East = 4,
        West = 8,
        NorthEast = North | East,
        NorthWest = North | West,
        SouthEast = South | East,
        SouthWest = South | West,
        NorthNorthWest = NorthWest | North << 4,
        NorthNorthEast = NorthEast | North << 4,
        EastNorthEast = NorthEast | East << 4,
        EastSouthEast = SouthEast | East << 4,
        SouthSouthEast = SouthEast | South << 4,
        SouthSouthWest = SouthWest | South << 4,
        WestSouthWest = SouthWest | West << 4,
        WestNorthWest = NorthWest | West << 4,
    }
}