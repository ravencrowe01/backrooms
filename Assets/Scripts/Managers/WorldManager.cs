using UnityEngine;

namespace Raven.backrooms.Managers {
    public class WorldManager : MonoBehaviour {
        public static WorldManager Instance { get; private set; }

        private void Awake () {
            if(Instance != null && this == Instance) {
                Destroy (this);
            }

            Instance = this;

            DontDestroyOnLoad (Instance);
        }

        void Start () {

        }

        void Update () {

        }
    }
}