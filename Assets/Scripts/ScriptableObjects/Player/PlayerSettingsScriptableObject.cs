using DG.Tweening;
using UnityEngine;

namespace EchoOfTheTimes.ScriptableObjects.Player
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Player/PlayerSettings", order = 1)]
    public class PlayerSettingsScriptableObject : ScriptableObject
    {
        [field: Header("Movement")]
        [field: SerializeField]
        public float MoveSpeed { get; private set; }
        [field: SerializeField]
        public float DistanceTreshold { get; private set; }


        [field: Header("Rotation")]
        [field: SerializeField]
        public float RotateDuration { get; private set; }
        [field: SerializeField]
        public AxisConstraint AxisConstraint { get; private set; }
    }
}