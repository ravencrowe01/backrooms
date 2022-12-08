using System.Collections.Generic;
using UnityEngine;

namespace Backrooms.Assets.Scripts.Pathfinding {
    public class Node {
        public int Cost { get; set; }

        public Vector2 Position { get; set; }

        public List<Direction> OpenEntrances { get; set; }

        public bool Blocking { get; set; }
    }
}
