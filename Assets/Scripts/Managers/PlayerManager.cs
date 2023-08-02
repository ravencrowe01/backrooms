using Raven.backrooms.World;
using UnityEngine;

namespace Raven.backrooms.Managers {
    public class PlayerManager : MonoBehaviour {
        public static PlayerManager Instance { get; private set; }

        public Player PlayerPrefab;
        private Player _player;
        private CharacterController _controller;

        [SerializeField]
        private float _playerSpeed = 2.0f;
        [SerializeField]
        private float _jumpHeight = 1.0f;
        [SerializeField]
        private float _gravityValue = -9.81f;

        private GameManager _gm;
        private InputManager _im;
        private Vector3 _playerVelocity;
        private bool _groundedPlayer;
        private Camera _camera;

        private void Awake () {
            if (Instance != null & this != Instance) {
                Destroy (this);
            }

            Instance = this;

            DontDestroyOnLoad (Instance);
        }

        private void Start () {
            _gm = GameManager.Instance;
            _im = _gm.InputManagerInstance;
            _player = Instantiate (PlayerPrefab);

            _controller = _player.GetComponent<CharacterController> ();
            _camera = _player.Camera;
        }

        void Update () {
            if (!_im.Locked) {
                MovePlayer ();
            }
        }

        private void MovePlayer () {
            _groundedPlayer = _controller.isGrounded;

            if (_groundedPlayer && _playerVelocity.y < 0) {
                _playerVelocity.y = 0f;
            }

            var movement = _im.GetPlayerMovement ();

            var move = new Vector3 (movement.x, 0, movement.y);

            move = _camera.transform.forward * move.z + _camera.transform.right * move.x;
            move.y = 0;

            _controller.Move (_playerSpeed * Time.deltaTime * move);

            if (_im.GetPlayerJumped () && _groundedPlayer) {
                _playerVelocity.y += Mathf.Sqrt (_jumpHeight * -3.0f * _gravityValue);
            }

            _playerVelocity.y += _gravityValue * Time.deltaTime;
            _controller.Move (_playerVelocity * Time.deltaTime);
        }

        public Camera GetCamera () => _camera;
    }
}
