using EchoOfTheTimes.ScriptableObjects;
using EchoOfTheTimes.Units;
using EchoOfTheTimes.Utils;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.Movement
{
    public class QuadrantCameraController : MonoBehaviour
    {
        public Transform Focus;

        private Player _player;
        private float _sensitivity;

        private Camera _camera;

        private float _focusCentering = 0.5f;
        private float _distance = 5f;
        private float _projectionSize = 9f;
        private float _focusRadius = 1f;

        private Vector3 _focusPoint;

        private float _autoRotationSpeed;

        private float _afkTime = 0f;
        private float _maxAfkTime_sec;
        private bool _isNeedAutoRotate = true;
        private bool _isAutoRotateTimerStart = false;

        // Новые переменные для фиксированных углов и плавного поворота
        private float[] _quadrantAngles = { 45f, 135f, 225f, 315f };
        private int _currentQuadrantIndex = 0;
        private float _currentYAngle;
        private float _rotationVelocity;
        private float _rotationSmoothTime = 0.5f; // Настройка для эффекта гидравлики

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

        private void LateUpdate()
        {
            UpdateFocusPoint();

            // Обновляем угол камеры
            UpdateCameraAngle();

            // Плавно поворачиваем камеру к целевому углу
            SmoothRotateToQuadrant();

            // Рассчитываем положение и поворот камеры
            Quaternion lookRotation = Quaternion.Euler(45f, _currentYAngle, 0f);
            Vector3 lookDirection = lookRotation * Vector3.forward;
            Vector3 lookPosition = _focusPoint - lookDirection * _distance;

            transform.SetPositionAndRotation(lookPosition, lookRotation);
        }

        [Inject]
        private void Construct(Player player, CameraSettingsScriptableObject cameraSettings)
        {
            _player = player;

            _sensitivity = cameraSettings.Sensitivity;
            _focusCentering = cameraSettings.FocusCentering;
            _distance = cameraSettings.Distance;
            _projectionSize = cameraSettings.ProjectionSize;
            _focusRadius = cameraSettings.FocusRadius;
            _autoRotationSpeed = cameraSettings.AutoRotationSpeed;
            _maxAfkTime_sec = cameraSettings.MaxAfkTime_sec;

            _camera = Camera.main;
            _camera.orthographicSize = _projectionSize;

            _focusPoint = Focus.position;

            // Инициализируем текущий угол Y первым фиксированным углом
            _currentYAngle = _quadrantAngles[_currentQuadrantIndex];
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

        private void UpdateCameraAngle()
        {
            // Рассчитываем угол между игроком и фокусом
            Vector3 directionToFocus = Focus.position - _player.transform.position;
            float calculatedAngle = Mathf.Atan2(directionToFocus.x, directionToFocus.z) * Mathf.Rad2Deg;

            if (calculatedAngle < 0f)
                calculatedAngle += 360f;

            // Определяем текущий квадрант
            int newQuadrantIndex = GetQuadrantIndex(calculatedAngle);

            if (newQuadrantIndex != _currentQuadrantIndex)
            {
                _currentQuadrantIndex = newQuadrantIndex;
            }
        }

        private int GetQuadrantIndex(float angle)
        {
            if (angle >= 0f && angle < 90f)
                return 0; // 45 градусов
            else if (angle >= 90f && angle < 180f)
                return 1; // 135 градусов
            else if (angle >= 180f && angle < 270f)
                return 2; // 225 градусов
            else
                return 3; // 315 градусов
        }

        private void SmoothRotateToQuadrant()
        {
            float targetAngle = _quadrantAngles[_currentQuadrantIndex];
            _currentYAngle = Mathf.SmoothDampAngle(_currentYAngle, targetAngle, ref _rotationVelocity, _rotationSmoothTime);
        }
    }
}
