using UnityEngine;

namespace Backrooms.Assets.Scripts.World {
    [CreateAssetMenu (fileName = "RoomSides", menuName = "Backrooms/RoomSides", order = 1)]
    public class RoomSides : ScriptableObject {
        public bool[] North;
        public bool[] South;
        public bool[] East;
        public bool[] West;
    }
}
