using DG.Tweening;
using System;
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

        [NonSerialized]
        private Sequence _sequence;

        public void AcceptState(StateParameter stateParameter = null, bool isDebug = false, TweenCallback onComplete = null)
        {
            _sequence = DOTween.Sequence();
            _sequence.OnComplete(onComplete);

            if (stateParameter != null)
            {
                SpecialBehaiour(stateParameter, isDebug);
            }
            else
            {
                DefaultBehaviour(isDebug, onComplete);
            }
        }

        private void DefaultBehaviour(bool isDebug, TweenCallback callback)
        {


            if (!isDebug)
            {
                //if (Target.position != Position)
                //{
                //    Target.DOMove(Position, 1f);
                //}
                //if (Target.rotation.eulerAngles != Rotation)
                //{
                //    Target.DORotate(Rotation, 1f);
                //}
                //if (Target.localScale != LocalScale)
                //{
                //    Target.DOScale(LocalScale, 1f);
                //}

                if (Target.position != Position)
                {
                    _sequence.Append(Target.DOMove(Position, 1f));
                }
                if (Target.rotation.eulerAngles != Rotation)
                {
                    _sequence.Append(Target.DORotate(Rotation, 1f));
                }
                if (Target.localScale != LocalScale)
                {
                    _sequence.Append(Target.DOScale(LocalScale, 1f));
                }
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
                if (stateParameter.Target.position != stateParameter.Position)
                {
                    stateParameter.Target.DOMove(stateParameter.Position, 1f);
                }
                if (stateParameter.Target.rotation.eulerAngles != stateParameter.Rotation)
                {
                    stateParameter.Target.DORotate(stateParameter.Rotation, 1f);
                }
                if (stateParameter.Target.localScale != stateParameter.LocalScale)
                {
                    stateParameter.Target.DOScale(stateParameter.LocalScale, 1f);
                }
            }
            else
            {
                stateParameter.Target.SetPositionAndRotation(stateParameter.Position, Quaternion.Euler(stateParameter.Rotation));
                stateParameter.Target.localScale = stateParameter.LocalScale;
            }
        }
    }
}