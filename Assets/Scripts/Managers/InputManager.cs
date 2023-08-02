using Raven.backrooms.Input;
using UnityEngine;

namespace Raven.backrooms.Managers {
    public class InputManager : MonoBehaviour {
        public static InputManager Instance { get; private set; }

        public bool Locked { get; private set; }

        public float VerticalRotSpeed { get; private set; } = 20f;
        public float HorizontalRotSpeed { get; private set; } = 20f;

        private PlayerInput _playerInput;

        private Vector2 _pcMovement = Vector2.zero;
        private bool _pcJumped = false;
        private Vector2 _pcRotate = Vector2.zero;

        private void Awake () {
            if (Instance != null && this == Instance) {
                Destroy (this);
            }

            Instance = this;

            DontDestroyOnLoad (Instance);

            _playerInput = new PlayerInput ();

            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update () {
            if (!Locked) {
                _pcMovement =  _playerInput.Default.PlayerMove.ReadValue<Vector2> ();
                _pcJumped = _playerInput.Default.PlayerJump.triggered;
                _pcRotate = _playerInput.Default.PlayerRotate.ReadValue<Vector2> ();
            }
        }

        private void OnEnable () {
            _playerInput.Enable ();
        }

        private void OnDisable () {
            _playerInput.Disable ();
        }

        public Vector2 GetPlayerMovement () => _pcMovement;
        public bool GetPlayerJumped () => _pcJumped;
        public Vector2 GetPlayerRotation () => _pcRotate;

        private void OnApplicationFocus (bool focus) {
            Locked = !focus;
        }
    }
}