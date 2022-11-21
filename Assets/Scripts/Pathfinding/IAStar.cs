using System.Collections.Generic;

namespace Backrooms.Assets.Scripts.Pathfinding {
    public interface IAStar {
        PathfindingStatus Step ();
    }
}