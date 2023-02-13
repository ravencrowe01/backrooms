using Backrooms.Assets.Scripts.World.Config;
using System.Collections.Generic;
using UnityEngine;

namespace Backrooms.Assets.Scripts.World {
    public class Room : MonoBehaviour {
        [SerializeField]
        private RoomSides _sides;

        public ID ID { 
            get {
                _id ??= new ID (_idM);

                return _id;
            }

            private set { 
                _id = value; 
            } 
        }
        private ID _id;

        [SerializeField]
        private int _idM;

        public IReadOnlyDictionary<Direction, ISideStateConfig> SideStates {
            get {
                if(_sideStates == null || _sideStates.Count == 0) {
                    if(_sides != null) {
                        _sideStates.Add (Direction.North, new SideStateConfig (_sides.North));
                        _sideStates.Add (Direction.South, new SideStateConfig (_sides.South));
                        _sideStates.Add (Direction.East, new SideStateConfig (_sides.East));
                        _sideStates.Add (Direction.West, new SideStateConfig (_sides.West));
                    }
                }

                return _sideStates;
            }
        }
        private Dictionary<Direction, ISideStateConfig> _sideStates;

        public Room () {
            _sideStates = new Dictionary<Direction, ISideStateConfig> ();
        }
    }
}
