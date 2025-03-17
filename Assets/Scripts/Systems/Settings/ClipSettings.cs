using UnityEngine;

namespace Systems.Settings
{
    [System.Serializable]
    public class ClipSettings
    {
        public AudioClip Clip;

        [field: Range(0f, 1f)]
        public float Volume = 1f;
    }
}
