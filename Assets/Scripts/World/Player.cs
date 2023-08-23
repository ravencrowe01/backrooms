using UnityEngine;

namespace Raven.backrooms.World {
    [RequireComponent (typeof (CharacterController))]
    public class Player : MonoBehaviour {
        public Camera Camera;
    }

    public Vector3 GetPos () => new Vector3();
}