using EchoOfTheTimes.Units;
using EchoOfTheTimes.Utils;
using System;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.Movement
{
    public class RefinedOrbitCamera : MonoBehaviour
    {
        public Transform Focus;

        private Player _player;
        public float Sensitivity;

        private Camera _camera;

        [SerializeField, Range(0f, 1f)]
        private float _focusCentering = 0.5f;
        [SerializeField]
        private float _distance = 5f;
        [SerializeField]
        private float _projectionSize = 9f;
        [SerializeField]
        private float _focusRadius = 1f;

        private Vector3 _focusPoint;
        private Vector2 _orbitAngles = new Vector2(45f, 0f);

        [Range(0f, 1f)]
        public float AutoRotationSpeed;

        private float _afkTime = 0f;
        public float MaxAfkTime_sec;
        private bool _isNeedAutoRotate = true;
        private bool _isAutoRotateTimerStart = false;

        private void OnDrawGizmos()
        {
            if (_camera != null)
            {
                var pc = _camera.transform.position;
                pc.y = 0f;
                var pp = _player.transform.position;
                pp.y = 0f;
                var pf = Focus.position;
                pf.y = 0f;

                GizmosHelper.DrawArrowBetween(_camera.transform.position, _player.transform.position, Color.yellow);
                GizmosHelper.DrawArrowBetween(pc, pp, Color.yellow);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(_camera.transform.position, _player.transform.position);
                Gizmos.DrawLine(pc, pp);
                GizmosHelper.DrawArrowBetween(_camera.transform.position, Focus.position, Color.yellow);
                GizmosHelper.DrawArrowBetween(pc, pf, Color.yellow);
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(_camera.transform.position, Focus.position);
                Gizmos.DrawLine(pc, pf);
                GizmosHelper.DrawArrowBetween(_player.transform.position, Focus.position, Color.yellow);
                GizmosHelper.DrawArrowBetween(pp, pf, Color.yellow);
                Gizmos.color = Color.green;
                Gizmos.DrawLine(_player.transform.position, Focus.position);
                Gizmos.DrawLine(pp, pf);

                Gizmos.color = Color.white;
                Gizmos.DrawLine(_camera.transform.position, pc);
                Gizmos.DrawLine(_player.transform.position, pp);
                Gizmos.DrawLine(Focus.position, pf);
            }
        }

        private void Awake()
        {
            //_camera = Camera.main;
            //_camera.orthographicSize = _projectionSize;

            //_focusPoint = Focus.position;
            //transform.localRotation = Quaternion.Euler(_orbitAngles);
        }

        private void FixedUpdate()
        {
            if (_isAutoRotateTimerStart)
            {
                _afkTime += Time.deltaTime;

                if (_afkTime > MaxAfkTime_sec)
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

            if (_player.IsBusy)
            {
                _isNeedAutoRotate = true;
            }

            if (_isNeedAutoRotate)
            {
                AutoRotateCamera();
            }
        }

        [Inject]
        private void Construct(Player player)
        {
            _player = player;

            _camera = Camera.main;
            _camera.orthographicSize = _projectionSize;

            _focusPoint = Focus.position;
            transform.localRotation = Quaternion.Euler(_orbitAngles);
        }

        private void AutoRotateCamera()
        {
            var pc = _camera.transform.position - _player.transform.position;
            var pf = _player.transform.position - Focus.position;
            pc.y = 0;
            pf.y = 0;
            var a = Vector3.SignedAngle(pf, pc, Vector3.up);
            float dir;

            if (a > 0f) dir = -1f;
            else dir = 1f;

            if (Mathf.Abs(a) > 0.1f)
            {
                Rotate(Mathf.Abs(a) * dir);
            }
        }

        public void ResetAfkTimer()
        {
            _afkTime = 0f;
        }

        private void UpdateFocusPoint()
        {
            Vector3 targetPoint = Focus.position;
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

        public void RotateCamera(float deltaX)
        {
            Rotate(deltaX);

            _isAutoRotateTimerStart = true;
            _isNeedAutoRotate = false;
            ResetAfkTimer();
        }

        private void Rotate(float deltaX)
        {
            transform.RotateAround(Focus.position, Vector3.up, deltaX * Sensitivity * Time.deltaTime);
        }
    }
}