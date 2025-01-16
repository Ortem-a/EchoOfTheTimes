using UnityEngine;

namespace Systems.Leveling
{
    public static class StateOptionExtensions
    {
        public static void ApplyStateOption(this Transform target, StateOption option)
        {
            target.SetLocalPositionAndRotation(option.LocalPosition, option.LocalRotation);
            target.localScale = option.LocalScale;
        }
    }
}