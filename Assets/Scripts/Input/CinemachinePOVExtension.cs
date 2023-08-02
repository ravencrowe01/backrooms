using Cinemachine;
using Raven.backrooms.Managers;
using UnityEngine;

namespace Raven.backrooms.Input {
    public class CinemachinePOVExtension : CinemachineExtension {
        [SerializeField]
        private float _yClampMin = 0f;

        [SerializeField]
        private float _yClampMax = 0f;

        private GameManager _gm;
        private InputManager _im;
        private Vector3 _startingRotation;

        protected override void Awake () {
            _gm = GameManager.Instance;
            _im = _gm.InputManagerInstance;
            _startingRotation = transform.localRotation.eulerAngles;
            base.Awake ();
        }

        protected override void PostPipelineStageCallback (CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime) {
            if (vcam.Follow) {
                if (stage == CinemachineCore.Stage.Aim) {
                    var deltaInput = _im.GetPlayerRotation ();

                    _startingRotation.x += (deltaInput.x * _im.HorizontalRotSpeed * Time.deltaTime);
                    _startingRotation.y += (-deltaInput.y * _im.VerticalRotSpeed * Time.deltaTime);
                    _startingRotation.y = Mathf.Clamp (_startingRotation.y, _yClampMin, _yClampMax);

                    state.RawOrientation = Quaternion.Euler (_startingRotation.y, _startingRotation.x, 0f);
                }
            }
        }
    }
}