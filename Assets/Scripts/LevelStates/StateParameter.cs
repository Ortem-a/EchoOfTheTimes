using DG.Tweening;
using EchoOfTheTimes.Core;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    [System.Serializable]
    public class StateParameter
    {
        public int StateId;
        public Transform Target;
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 LocalScale;

        [System.NonSerialized]
        private int _defaultCompleteCounter = 0;
        [System.NonSerialized]
        private int _specialCompleteCounter = 0;
        [System.NonSerialized]
        private int _completeChecker = 3;
        [System.NonSerialized]
        private TweenCallback _onComplete;

        public void AcceptState(StateParameter stateParameter = null, bool isDebug = false, TweenCallback onComplete = null)
        {
            _onComplete = onComplete;

            //ChangeColorByState(stateParameter);

            if (stateParameter != null)
            {
                _specialCompleteCounter = 0;
                SpecialBehaiour(stateParameter, isDebug);
            }
            else
            {
                _defaultCompleteCounter = 0;
                DefaultBehaviour(isDebug);
            }
        }

        private void DefaultBehaviour(bool isDebug)
        {
            if (!isDebug)
            {
                Target.DOMove(Position, GameManager.Instance.TimeToChangeState_sec)
                    .OnComplete(() => OnCompleteDefaultTransformation());
                Target.DORotate(Rotation, GameManager.Instance.TimeToChangeState_sec)
                    .OnComplete(() => OnCompleteDefaultTransformation());
                Target.DOScale(LocalScale, GameManager.Instance.TimeToChangeState_sec)
                    .OnComplete(() => OnCompleteDefaultTransformation());
            }
            else
            {
                Target.SetPositionAndRotation(Position, Quaternion.Euler(Rotation));
                Target.localScale = LocalScale;
            }
        }

        private void SpecialBehaiour(StateParameter stateParameter, bool isDebug)
        {
            if (!isDebug)
            {
                stateParameter.Target.DOMove(stateParameter.Position, GameManager.Instance.TimeToChangeState_sec)
                    .OnComplete(() => OnCompleteSpecialTransformation());
                stateParameter.Target.DORotate(stateParameter.Rotation, GameManager.Instance.TimeToChangeState_sec)
                    .OnComplete(() => OnCompleteSpecialTransformation());
                stateParameter.Target.DOScale(stateParameter.LocalScale, GameManager.Instance.TimeToChangeState_sec)
                    .OnComplete(() => OnCompleteSpecialTransformation());
            }
            else
            {
                stateParameter.Target.SetPositionAndRotation(stateParameter.Position, Quaternion.Euler(stateParameter.Rotation));
                stateParameter.Target.localScale = stateParameter.LocalScale;
            }
        }

        private void OnCompleteDefaultTransformation()
        {
            _defaultCompleteCounter++;

            if (_defaultCompleteCounter == _completeChecker)
            {
                _onComplete?.Invoke();
            }
        }

        private void OnCompleteSpecialTransformation()
        {
            _specialCompleteCounter++;

            if (_specialCompleteCounter == _completeChecker)
            {
                _onComplete?.Invoke();
            }
        }

        private void ChangeColorByState(StateParameter stateParameter = null)
        {
            if (stateParameter == null) 
            {
                var color = GameManager.Instance.ColorStateSettings.GetColor(StateId);

                if (Target.gameObject.TryGetComponent(out Renderer renderer))
                {
                    renderer.material.color = color;
                }
                else
                {
                    var renderers = Target.GetComponentsInChildren<Renderer>();

                    if (renderers != null && renderers.Length > 0)
                    {
                        foreach (var r in renderers)
                        {
                            r.material.color = color;
                        }
                    }
                }
            }
            else
            {
                var color = GameManager.Instance.ColorStateSettings.GetColor(stateParameter.StateId);

                if (stateParameter.Target.gameObject.TryGetComponent(out Renderer renderer))
                {
                    renderer.material.color = color;
                }
                else
                {
                    var renderers = stateParameter.Target.GetComponentsInChildren<Renderer>();

                    if (renderers != null && renderers.Length > 0)
                    {
                        foreach (var r in renderers)
                        {
                            r.material.color = color;
                        }
                    }
                }
            }
        }
    }
}