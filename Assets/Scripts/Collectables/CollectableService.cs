using EchoOfTheTimes.Persistence;
using EchoOfTheTimes.UI;
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

        private PlayerProgressHudView _playerProgressHudView;

        private void Awake()
        {
            var persistenceService = FindObjectOfType<PersistenceService>();

            _collectedEarlier = persistenceService != null 
                ? persistenceService.GetLevel(ChapterTitle, LevelName).Collected
                : 0;
        
            _totalCollectables = GetComponent<CollectableSpawner>().NumberOfPlaceholders;

            _playerProgressHudView = FindObjectOfType<PlayerProgressHudView>();
        }

        public void OnCollected()
        {
            _collectedCollectables++;

            if (_collectedCollectables > _collectedEarlier)
            {
                _collectedEarlier++;
            }

            _playerProgressHudView.UpdateProgress(_collectedCollectables);
        }

#if UNITY_EDITOR
        public void SetTotalCollectables(int total)
        {
            _totalCollectables = total;
        }
#endif
    }
}