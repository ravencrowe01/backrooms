using System.Collections.Generic;
using UnityEngine;

namespace Backrooms.Assets.Scripts.World.Config {
    public interface IRoomConfig {
        IReadOnlyDictionary<Direction, ISideStateConfig> SideStates { get; }
        Vector2 Coordinates { get; }

        IDictionary<Direction, ISideStateConfig> GetOpenSides ();
    }
}