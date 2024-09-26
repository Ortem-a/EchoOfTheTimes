using EchoOfTheTimes.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.LevelStates
{
    [RequireComponent(typeof(MovableByButtonGizmosDrawer))]
    public class MovableByButton : MonoBehaviour
    {
        public bool IsLocalSpace;

        // Список для хранения параметров состояния по индексам
        [SerializeField]
        private List<StateParameter> _indexedStates = new List<StateParameter>();

        // Текущий индекс выбранного состояния
        [SerializeField]
        private int _currentStateIndex = 0;

        // Свойство для доступа к текущему индексу
        public int CurrentStateIndex
        {
            get => _currentStateIndex;
            set => _currentStateIndex = Mathf.Clamp(value, 0, _indexedStates.Count - 1); // Ограничиваем индекс
        }

        private StateService _stateService;

        [Inject]
        private void Construct(StateService stateService)
        {
            _stateService = stateService;
        }

        public virtual void Move(Action onComplete)
        {
            if (_currentStateIndex < 0 || _currentStateIndex >= _indexedStates.Count)
            {
                Debug.LogError($"Invalid state index: {_currentStateIndex}");
                return;
            }
            _stateService.AcceptState(_indexedStates[_currentStateIndex], onComplete: () => onComplete?.Invoke());
        }

#if UNITY_EDITOR
        public void SetOrUpdateParams()
        {
            Vector3 newPosition = transform.position;
            Vector3 newRotation = transform.rotation.eulerAngles;
            if (IsLocalSpace)
            {
                newPosition = transform.localPosition;
                newRotation = transform.localRotation.eulerAngles;
            }

            var newStateParam = new StateParameter
            {
                StateId = CurrentStateIndex,
                Target = transform,
                Position = newPosition,
                Rotation = newRotation,
                LocalScale = transform.localScale,
                IsLocalSpace = IsLocalSpace
            };

            // Обновляем или добавляем состояние
            if (CurrentStateIndex < _indexedStates.Count)
            {
                _indexedStates[CurrentStateIndex] = newStateParam; // Перезаписываем существующий
            }
            else
            {
                _indexedStates.Add(newStateParam); // Добавляем новый
            }
        }

        public void TransformObjectByParams()
        {
            if (_currentStateIndex < 0 || _currentStateIndex >= _indexedStates.Count)
            {
                Debug.LogError($"Invalid state index: {_currentStateIndex}");
                return;
            }

            var parameter = _indexedStates[_currentStateIndex];
            if (IsLocalSpace)
            {
                transform.SetLocalPositionAndRotation(parameter.Position, Quaternion.Euler(parameter.Rotation));
            }
            else
            {
                transform.SetPositionAndRotation(parameter.Position, Quaternion.Euler(parameter.Rotation));
            }
            transform.localScale = parameter.LocalScale;
        }
#endif
    }
}
