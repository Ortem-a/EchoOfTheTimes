using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.Movement
{
    public class TargetFollower : MonoBehaviour
    {
        public Vector3 PositionOffset;
        public Vector3 Rotation;

        public Transform Target;

        private void OnValidate()
        {
            FollowForTarget();
        }

        private void LateUpdate()
        {
            FollowForTarget();
        }

        private void FollowForTarget()
        {
            transform.SetPositionAndRotation(
                Target.position + PositionOffset, 
                Quaternion.Euler(Rotation));
        }
    }
}