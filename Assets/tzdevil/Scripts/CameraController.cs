using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace tzdevil.Gameplay
{
    public class CameraController : MonoBehaviour
    {
        private Mouse _mouse;

        private const float MIN_ZOOM = 4.5f;
        private const float MAX_ZOOM = 10f;

        [Header("References")]
        [SerializeField] private Transform _cinemachineTransform;
        [SerializeField] private CinemachineCamera _cinemachine;
        [SerializeField] private Transform _cameraRotationTransform;

        [Header("Zoom Settings")]
        [SerializeField] private float _currentZoomValue;
        [SerializeField] private float _zoomMultiplier;
        [SerializeField] private float _zoomVelocity;
        [SerializeField] private float _smoothTime;

        [Header("Game Loop")]
        [SerializeField] private bool _cameraCurrentlyInUse; // some other effect is using camera

        [Header("Movement Settings")]
        [SerializeField] private float _movementSpeed;

        [Header("Movement Values")]
        [SerializeField] private bool _holdingMiddleMouse;
        [SerializeField] private Vector2 _movementInput;

        [Header("Rotation")]
        [SerializeField] private float _currentRotationValue;

        private void Awake()
        {
            _mouse = Mouse.current;
        }

        private void Start()
        {
            _currentZoomValue = _cinemachine.Lens.OrthographicSize;
        }

        private void Update()
        {
            if (_cameraCurrentlyInUse)
                return;

            MoveWithMouse();

            MoveWithKeys();

            ZoomWithScrollWheel();
        }

        public void MoveScreen(InputAction.CallbackContext context)
        {
            _movementInput = context.ReadValue<Vector2>();
        }

        // TODO yarın: bunları keybind'la.
        public void ResetCamera(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _cinemachine.transform.DOMove(Vector3.zero, .4f);

                DOTween.To(() => _currentZoomValue, x => _currentZoomValue = x, 4f, .2f).OnUpdate(() =>
                {
                    _cinemachine.Lens.OrthographicSize = _currentZoomValue;
                });
            }
        }

        public void RotateCameraToLeft(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _currentRotationValue += 90;

                if (_currentRotationValue == 360)
                    _currentRotationValue = 0;

                _cameraRotationTransform.DORotate(new Vector3(0, _currentRotationValue, 0), 0.25f);
            }
        }

        public void RotateCameraToRight(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _currentRotationValue -= 90;

                if (_currentRotationValue == -360)
                    _currentRotationValue = 0;

                _cameraRotationTransform.DORotate(new Vector3(0, _currentRotationValue, 0), 0.25f);
            }
        }

        private void MoveWithKeys()
        {
            Vector3 left = -_cinemachineTransform.right;
            Vector3 down = -_cinemachineTransform.up;

            Vector3 moveDirection = (left * _movementInput.x + down * _movementInput.y).normalized;

            Vector3 movement = _movementSpeed * Time.deltaTime * moveDirection;
            var finalPos = _cinemachine.transform.position - movement;
            //_cinemachine.transform.position = new Vector3(Mathf.Clamp(finalPos.x, -18, 18), Mathf.Clamp(finalPos.y, 0, 4), Mathf.Clamp(finalPos.z, -18, 18));
            _cinemachine.transform.position = finalPos;
        }

        public void MoveTrigger(InputAction.CallbackContext context)
        {
            if (context.started)
                _holdingMiddleMouse = true;
            else if (context.canceled)
                _holdingMiddleMouse = false;
        }

        private void MoveWithMouse()
        {
            if (_holdingMiddleMouse)
            {
                float horizontalInput = _mouse.delta.x.ReadValue();
                float verticalInput = _mouse.delta.y.ReadValue();

                Vector3 right = _cinemachineTransform.right;
                Vector3 up = _cinemachineTransform.up;

                Vector3 moveDirection = (right * horizontalInput + up * verticalInput).normalized;

                Vector3 movement = _movementSpeed * Time.deltaTime * moveDirection;
                var finalPos = _cinemachine.transform.position - movement;
                //_cinemachine.transform.position = new Vector3(Mathf.Clamp(finalPos.x, -18, 18), Mathf.Clamp(finalPos.y, 0, 4), Mathf.Clamp(finalPos.z, -18, 18));
                _cinemachine.transform.position = finalPos;
            }
        }

        private void ZoomWithScrollWheel()
        {
            float scroll = _mouse.scroll.ReadValue().y;
            _currentZoomValue = Mathf.Clamp(_currentZoomValue - scroll * _zoomMultiplier, MIN_ZOOM, MAX_ZOOM);
            _cinemachine.Lens.OrthographicSize = Mathf.SmoothDamp(_cinemachine.Lens.OrthographicSize, _currentZoomValue, ref _zoomVelocity, _smoothTime);

            if (scroll == 0)
                _movementSpeed = 20 - (MAX_ZOOM - _currentZoomValue);
        }

        public void SetPositionAndZoom(Vector3 position, float zoomValue)
        {
            _cinemachine.transform.position = position;
            _currentZoomValue = zoomValue;
            _cinemachine.Lens.OrthographicSize = zoomValue;
        }

        public void ChangeCameraUseValue(bool use)
        {
            _cameraCurrentlyInUse = use;
        }
    }
}