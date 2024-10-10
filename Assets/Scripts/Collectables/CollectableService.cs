using EchoOfTheTimes.Persistence;
using UnityEngine;

namespace EchoOfTheTimes.Collectables
{
    [RequireComponent(typeof(CollectableSpawner))]
    public class CollectableService : MonoBehaviour
    {
        public int CollectedResult => _collectedEarlier;

        public string LevelFullName = "Chapter|Level";
        public string ChapterTitle => LevelFullName.Split('|')[0];
        public string LevelName => LevelFullName.Split('|')[1];

        [SerializeField]
        private int _collectedEarlier;
        [SerializeField]
        private int _collectedCollectables = 0;

        [SerializeField]
        private int _totalCollectables;

        private CollectableSpawner _spawner;
        private PersistenceService _persistenceService;

        private void Awake()
        {
            _spawner = GetComponent<CollectableSpawner>();
            _totalCollectables = _spawner.NumberOfPlaceholders;

            _persistenceService = FindObjectOfType<PersistenceService>();
            _collectedEarlier = _persistenceService.GetLevel(ChapterTitle, LevelName).Collected;
        }

        public void OnCollected()
        {
            _collectedCollectables++;

            if (_collectedCollectables > _collectedEarlier)
            {
                _collectedEarlier++;
            }
        }

#if UNITY_EDITOR
        public void SetTotalCollectables(int total)
        {
            _totalCollectables = total;
        }
#endif
    }
}