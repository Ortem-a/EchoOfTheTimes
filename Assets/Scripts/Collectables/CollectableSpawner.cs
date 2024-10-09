using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.Collectables
{
    public class CollectableSpawner : MonoBehaviour
    {
        [SerializeField]
        private List<CollectablePlaceholder> _placeholders;

        [SerializeField]
        private Collectable _prefab;

        public int NumberOfPlaceholders => _placeholders.Count;

        private void Awake()
        {
            foreach (var placeholder in _placeholders)
            {
                Instantiate(_prefab, placeholder.transform);
            }
        }

#if UNITY_EDITOR
        public void SetPlaceholdersFromEditor(List<CollectablePlaceholder> placeholders)
        {
            _placeholders = placeholders;
        }
#endif
    }
}