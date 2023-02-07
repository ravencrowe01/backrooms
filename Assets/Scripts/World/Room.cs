using Backrooms.Assets.Scripts.World.Config;
using System.Collections.Generic;
using UnityEngine;

namespace Backrooms.Assets.Scripts.World {
    public class Room : MonoBehaviour {
        [SerializeField]
        private RoomSides _sides;

        public ID ID { get; private set; }
        [SerializeField]
        private int _idM;

        public IReadOnlyDictionary<Direction, ISideStateConfig> SideStates => _sideStates;
        private Dictionary<Direction, ISideStateConfig> _sideStates;

        public Room () {
            _sideStates = new Dictionary<Direction, ISideStateConfig> ();
        }

        private void Awake () {
            _sideStates.Add (Direction.North, new SideStateConfig( _sides.North));
            _sideStates.Add (Direction.South, new SideStateConfig (_sides.South));
            _sideStates.Add (Direction.East, new SideStateConfig (_sides.East));
            _sideStates.Add (Direction.West, new SideStateConfig (_sides.West));

            ID = new ID (_idM);
        }
    }
}
