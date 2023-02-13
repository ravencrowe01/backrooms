using UnityEngine;

namespace Backrooms.Assets.Scripts.Pathfinding {
    public interface IAStar {
        Vector2 Current { get; }

        PathfindingStatus Step ();
    }
}