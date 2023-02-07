using System.Collections.Generic;
using System.Numerics;

namespace Backrooms.Assets.Scripts.World.Config {
    public interface IRoomConfig {
        IReadOnlyDictionary<Direction, ISideStateConfig> SideStates { get; }
        Vector2 Coordinates { get; }

        IEnumerable<Direction> GetOpenSides ();
    }
}