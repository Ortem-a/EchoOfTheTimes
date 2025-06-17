using UnityEngine;

namespace SystemsEditor
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Editor/VertexGizmosColorSettings", order = 2)]
    public class VertexGizmosColorSettingsScriptableObject : ScriptableObject
    {
        [field: SerializeField]
        public Color32 Static { get; private set; }
        [field: SerializeField]
        public Color32 Moving { get; private set; }
        [field: SerializeField]
        public Color32 Bridge { get; private set; }
        [field: SerializeField]
        public Color32 Connection { get; private set; }
        [field: SerializeField]
        public Color32 Arrow { get; private set; }
        [field: SerializeField]
        public Color32 Text { get; private set; }
    }
}