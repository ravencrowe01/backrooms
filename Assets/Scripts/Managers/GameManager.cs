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

        private bool _shutdownRequested = false;

        private void Awake () {
            if (Instance != null&& Instance != this) {
                Destroy (this);
            }

            Instance = this;

            DontDestroyOnLoad (Instance);

            InputManagerInstance = Instantiate (InputManagerPrefab);
            WorldManagerInstance = Instantiate (WorldManagerPrefab);
        }

        private void Start () {

        }

        private void Update () {

        }

        private void LateUpdate () {
            if (_shutdownRequested) {
                BeginShutdown ();
                Shutdown ();
            }
        }

        private void BeginShutdown () {
            // For cleanup that needs to be done before shutdown
        }

        private void Shutdown () {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit ();
        }

        public void RequestShutdown (object requester) {
            _shutdownRequested = true;
        }
    }
}