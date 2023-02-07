using Backrooms.Assets.Scripts.World.Config;
using System.Numerics;

namespace Backrooms.Assets.Scripts.Pathfinding {
    public class Node {
        public int Cost { get; set; }

        public Vector2 Position { get; set; }

        public IRoomConfig Config { get; set; }

        public bool Blocking { get; set; }
    }
}
