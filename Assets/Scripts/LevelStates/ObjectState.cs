using DG.Tweening;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    [System.Serializable]
    public class ObjectState
    {
        public Transform Target;

        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 LocalScale;

        public void AcceptState()
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
    }
}