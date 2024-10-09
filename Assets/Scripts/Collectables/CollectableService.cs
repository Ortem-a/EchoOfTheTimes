using EchoOfTheTimes.Persistence;
using UnityEngine;

namespace EchoOfTheTimes.Collectables
{
    [RequireComponent(typeof(CollectableSpawner))]
    public class CollectableService : MonoBehaviour
    {
        [SerializeField]
        private int _collectedCollectables;

        [SerializeField]
        private int _totalCollectables;

        private CollectableSpawner _spawner;
        private PersistenceService _persistenceService;

        private void Awake()
        {
            _spawner = GetComponent<CollectableSpawner>();
            _totalCollectables = _spawner.NumberOfPlaceholders;

            _collectedCollectables = _persistenceService.GetLastLoadedLevel().Collected;
        }
    }
}