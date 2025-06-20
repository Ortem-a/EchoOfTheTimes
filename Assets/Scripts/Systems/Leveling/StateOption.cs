using System.Text;
using UnityEngine;

namespace Systems.Leveling
{
    [System.Serializable]
    public class StateOption
    {
        public Vector3 LocalPosition;
        public Quaternion LocalRotation;
        public Vector3 LocalScale;

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"{nameof(LocalPosition)}: {LocalPosition}");
            sb.AppendLine($"{nameof(LocalRotation)}: {LocalRotation.eulerAngles}");
            sb.AppendLine($"{nameof(LocalScale)}: {LocalScale}");

            return sb.ToString();
        }

        public static explicit operator StateOption(Transform target)
        {
            return new StateOption()
            {
                LocalPosition = target.localPosition,
                LocalRotation = target.localRotation,
                LocalScale = target.localScale
            };
        }
    }
}