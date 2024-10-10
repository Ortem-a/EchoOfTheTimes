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
            var serivce = GetComponent<CollectableService>();

            foreach (var placeholder in _placeholders)
            {
                var collectable = Instantiate(_prefab, placeholder.transform);
                collectable.Initialize(serivce);
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