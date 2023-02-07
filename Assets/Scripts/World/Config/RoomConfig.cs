using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Backrooms.Assets.Scripts.World.Config {
    public class RoomConfig : IRoomConfig {
        public Vector2 Coordinates => new Vector2 (_cords.X, _cords.Y);
        private Vector2 _cords;

        public IReadOnlyDictionary<Direction, ISideStateConfig> SideStates { get; private set; }

        public RoomConfig (Vector2 cords, Dictionary<Direction, ISideStateConfig> sideStates) {
            _cords = cords;
            SideStates = sideStates;
        }

        public IEnumerable<Direction> GetOpenSides () => SideStates.Keys.Where (d => SideStates[d].Open);
    }
}
