using System.Text;
using UnityEngine;

namespace Systems.Leveling
{
    [System.Serializable]
    public class StateOption
    {
        public Transform Target;

        public Vector3 LocalPosition;
        public Quaternion LocalRotation;
        public Vector3 LocalScale;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{nameof(Target)}: {Target.name}\n");
            sb.Append($"{nameof(LocalPosition)}: {LocalPosition}\n");
            sb.Append($"{nameof(LocalRotation)}: {LocalRotation.eulerAngles}\n");
            sb.Append($"{nameof(LocalScale)}: {LocalScale}\n");

            return sb.ToString();
        }
    }
}