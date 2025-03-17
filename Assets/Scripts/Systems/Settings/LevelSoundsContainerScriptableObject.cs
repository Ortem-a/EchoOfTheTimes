using UnityEngine;

namespace Systems.Settings
{
    [CreateAssetMenu(
        fileName = "New Level Sounds Container (Scriptable Object)",
        menuName = "Settings/Level Sounds Container", order = 3)]
    public class LevelSoundsContainerScriptableObject : ScriptableObject
    {
        [field: SerializeField]
        public ClipSettings AmbientSound { get; private set; }
        [field: SerializeField]
        public ClipSettings ChangeStateSound { get; private set; }
        [field: SerializeField]
        public ClipSettings MovableByRulesObjectsSound { get; private set; }
        [field: SerializeField]
        public ClipSettings LevelButtonPilinkSound { get; private set; }
        [field: SerializeField]
        public ClipSettings LevelButtonChangeSound { get; private set; }
        [field: SerializeField]
        public ClipSettings TeleportSound { get; private set; }
    }
}
