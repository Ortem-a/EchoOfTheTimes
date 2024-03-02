using DG.Tweening;
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

        public void AcceptState(StateParameter stateParameter = null)
        {
            if (stateParameter != null)
            {
                SpecialBehaiour(stateParameter);
            }
            else
            {
                DefaultBehaviour();
            }
        }

        private void DefaultBehaviour()
        {
            if (Target.position != Position)
            {
                Target.DOMove(Position, 1f);
            }
            if (Target.rotation.eulerAngles != Rotation)
            {
                Target.DORotate(Rotation, 1f);
            }
            if (Target.localScale != LocalScale)
            {
                Target.DOScale(LocalScale, 1f);
            }
        }

        private void SpecialBehaiour(StateParameter stateParameter)
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
    }
}