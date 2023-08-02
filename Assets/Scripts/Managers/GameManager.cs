using UnityEngine;

namespace Raven.backrooms.Managers {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance { get; private set; }

        public InputManager InputManagerPrefab;
        public InputManager InputManagerInstance { get; private set; }

        public WorldManager WorldManagerPrefab;
        public WorldManager WorldManagerInstance { get; private set; }

        public PlayerManager PlayerManagerPrefab;
        public PlayerManager PlayerManagerInstance { get; private set; }

        private void Awake () {
            if (Instance != null&& Instance != this) {
                Destroy (this);
            }

            Instance = this;

            DontDestroyOnLoad (Instance);

            InputManagerInstance = Instantiate (InputManagerPrefab);
            WorldManagerInstance = Instantiate (WorldManagerPrefab);
            PlayerManagerInstance = Instantiate (PlayerManagerPrefab);
        }

        void Start () {

        }

        void Update () {

        }
    }
}