using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.Collectables
{
    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField]
        private List<ItemFactory> _presets;

        private void Awake()
        {
            if (_presets != null)
            {
                foreach (var preset in _presets)
                {
                    preset.Create();
                }
            }
        }
    }
}