using UnityEngine;

namespace Backrooms.Assets.Scripts {
    [RequireComponent (typeof (CharacterController))]
    public class PlayerControl : MonoBehaviour {
        public float WalkingSpeed = 7.5f;
        public float RunningSpeed = 11.5f;
        public float JumpSpeed = 8.0f;
        public float Gravity = 20.0f;
        public Camera PlayerCamera;
        public float LookSpeed = 2.0f;
        public float LookXLimit = 70.0f;

        private CharacterController CharacterController;
        private Vector3 _moveDirection = Vector3.zero;
        private float _rotationX = 0;

        [HideInInspector]
        public bool CanMove = true;

        private void Start () {
            CharacterController = GetComponent<CharacterController> ();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update () {
            if (Input.GetKeyDown (KeyCode.R)) {
                Cursor.lockState = Cursor.lockState == CursorLockMode.Confined ? CursorLockMode.None : CursorLockMode.Confined;
                Cursor.visible = !Cursor.visible;

                CanMove = !CanMove;
            }

            if (CanMove) {
                MovePlayer ();
            }
        }

        private void MovePlayer () {
            Vector3 forward = transform.TransformDirection (Vector3.forward);
            Vector3 right = transform.TransformDirection (Vector3.right);
            float curSpeedX = WalkingSpeed * Input.GetAxis ("Vertical");
            float curSpeedY = WalkingSpeed * Input.GetAxis ("Horizontal");
            float movementDirectionY = _moveDirection.y;

            _moveDirection = forward * curSpeedX + right * curSpeedY;

            if (Input.GetButton ("Jump") && CanMove && CharacterController.isGrounded) {
                _moveDirection.y = JumpSpeed;
            }
            else {
                _moveDirection.y = movementDirectionY;
            }

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            if (!CharacterController.isGrounded) {
                _moveDirection.y -= Gravity * Time.deltaTime;
            }

            CharacterController.Move (_moveDirection * Time.deltaTime);

            if (CanMove) {
                _rotationX += -Input.GetAxis ("Mouse Y") * LookSpeed;
                _rotationX = Mathf.Clamp (_rotationX, -LookXLimit, LookXLimit);
                PlayerCamera.transform.localRotation = Quaternion.Euler (_rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler (0, Input.GetAxis ("Mouse X") * LookSpeed, 0);
            }
        }
    }
}