using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Backrooms.Assets.Scripts.World.Config {
    public class RoomConfig : IRoomConfig {
        public Vector2 Coordinates => new Vector2 (_cords.x, _cords.y);
        private Vector2 _cords;

        public IReadOnlyDictionary<Direction, ISideStateConfig> SideStates { get; private set; }

        public RoomConfig (Vector2 cords, Dictionary<Direction, ISideStateConfig> sideStates) {
            _cords = cords;
            SideStates = sideStates;
        }

        public IDictionary<Direction, ISideStateConfig> GetOpenSides () {
            var dict = new Dictionary<Direction, ISideStateConfig> ();

            foreach(var dir in SideStates.Keys) {
                if (SideStates[dir].Open) {
                    dict.Add (dir, SideStates[dir]);
                }
            }

            return dict;
        }
    }
}
