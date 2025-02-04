using Systems.Settings;
using Systems.Units;
using UnityEngine;
using Zenject;

namespace Systems.Inputs
{
    public class RefinedOrbitCamera : MonoBehaviour
    {
        private Transform _focus;

        private IUnit _target;
        private float _sensitivity;

        private Camera _camera;

        private float _focusCentering;
        private float _distance;
        private float _projectionSize;
        private float _focusRadius;

        private Vector3 _focusPoint;
        private Vector2 _orbitAngles;

        private float _autoRotationSpeed;

        private float _afkTime;
        private float _maxAfkTime_sec;
        private bool _isNeedAutoRotate = true;
        private bool _isAutoRotateTimerStart = false;

        [Inject]
        private void Construct(CameraSettingsScriptableObject cameraSettings)
        {
            _sensitivity = cameraSettings.Sensitivity;
            _focusCentering = cameraSettings.FocusCentering;
            _distance = cameraSettings.Distance;
            _projectionSize = cameraSettings.ProjectionSize;
            _focusRadius = cameraSettings.FocusRadius;
            _orbitAngles = cameraSettings.OrbitAngles;
            _autoRotationSpeed = cameraSettings.AutoRotationSpeed;
            _maxAfkTime_sec = cameraSettings.MaxAfkTime_sec;

            _camera = Camera.main;
            _camera.orthographicSize = _projectionSize;

            _focus = FindObjectOfType<CameraFocusMarker>().transform;
            _focusPoint = _focus.position;
            transform.localRotation = Quaternion.Euler(_orbitAngles);
        }

        private void FixedUpdate()
        {
            if (_isAutoRotateTimerStart)
            {
                _afkTime += Time.deltaTime;

                if (_afkTime > _maxAfkTime_sec)
                {
                    _isNeedAutoRotate = true;
                    _isAutoRotateTimerStart = false;
                }
            }
        }

        private void LateUpdate()
        {
            UpdateFocusPoint();

            var lookRotation = transform.localRotation;
            Vector3 lookDirection = lookRotation * Vector3.forward;
            Vector3 lookPosition = _focusPoint - lookDirection * _distance;

            transform.SetPositionAndRotation(lookPosition, lookRotation);

            if (_isNeedAutoRotate)
            {
                AutoRotateCamera();
            }
        }

        public void SetTarget(IUnit target)
        {
            _target = target;
        }

        private void AutoRotateCamera()
        {
            var pc = _camera.transform.position - _target.Transform.position;
            var pf = _target.Transform.position - _focus.position;
            pc.y = 0;
            pf.y = 0;
            var a = Vector3.SignedAngle(pf, pc, Vector3.up);
            float dir;

            if (a > 0f) dir = -1f;
            else dir = 1f;

            if (Mathf.Abs(a) > 0.1f)
            {
                Rotate(Mathf.Abs(a) * dir * _autoRotationSpeed * 3f);
            }
        }

        private void UpdateFocusPoint()
        {
            Vector3 targetPoint = _focus.position;
            if (_focusRadius > 0f)
            {
                float distance = Vector3.Distance(targetPoint, _focusPoint);

                float t = 1f;
                if (distance > 0.01f && _focusCentering > 0f)
                {
                    t = Mathf.Pow(1f - _focusCentering, Time.unscaledDeltaTime);
                }

                if (distance > _focusRadius)
                {
                    t = Mathf.Min(t, _focusRadius / distance);
                }

                _focusPoint = Vector3.Lerp(targetPoint, _focusPoint, t);
            }
            else
            {
                _focusPoint = targetPoint;
            }
        }

        private void Rotate(float angle)
        {
            transform.RotateAround(_focus.position, Vector3.up, angle * Time.deltaTime);
        }
    }
}