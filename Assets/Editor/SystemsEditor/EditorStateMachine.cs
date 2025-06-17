using Systems.Leveling;
using UnityEditor;

namespace SystemsEditor
{
    public static class EditorStateMachine
    {
        [UnityEditor.MenuItem("Level States/State 0")]
        private static void SwitchState0() => SwitchToState(0);

        [UnityEditor.MenuItem("Level States/State 1")]
        private static void SwitchState1() => SwitchToState(1);

        [UnityEditor.MenuItem("Level States/State 2")]
        private static void SwitchState2() => SwitchToState(2);

        [UnityEditor.MenuItem("Level States/State 3")]
        private static void SwitchState3() => SwitchToState(3);

        private static void SwitchToState(int stateId)
        {
            var stateables = UnityEngine.Object.FindObjectsOfType<Stateable>();

            foreach (Stateable stateable in stateables)
            {
                if (stateable.TryGetOption(stateId, out var option))
                {
                    stateable.transform.SetLocalPositionAndRotation(
                        option.LocalPosition, option.LocalRotation);
                    stateable.transform.localScale = option.LocalScale;
                }
            }
        }
    }
}
