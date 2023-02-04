using Raven.Backrooms.Framework;
using Raven.Backrooms.Framework.Word.Config;
using System.Collections.Generic;
using UnityEngine;

namespace Backrooms.Assets.Scripts.World {
    public class Room : MonoBehaviour {
        public struct DirectionSide {
            public Direction Direction;
            public ISideStateConfig SideState;
        }

        [SerializeField]
        private DirectionSide[] _states;

        public IReadOnlyDictionary<Direction, ISideStateConfig> SideStates => _sideStates;
        private Dictionary<Direction, ISideStateConfig> _sideStates;

        public Room () {
            _sideStates = new Dictionary<Direction, ISideStateConfig> ();
        }

        private void Awake () {
            foreach(var state in _states) {
                _sideStates[state.Direction] = state.SideState;
            }
        }
    }
}
