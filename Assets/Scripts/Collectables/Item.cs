using EchoOfTheTimes.Interfaces;
using UnityEngine;

namespace EchoOfTheTimes.Collectables
{
    [RequireComponent(typeof(SphereCollider), typeof(CollectableEffectsService))]
    public class Item : MonoBehaviour, ICollectable, ISpawnable
    {
        public int Id { get; private set; }

        public CollectableStatusType Status { get; private set; }

        private SphereCollider _collider;
        private CollectableEffectsService _effectsService;
        private ItemSpawner _spawner;

        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
            _collider.isTrigger = true;

            _effectsService = GetComponent<CollectableEffectsService>();
            _effectsService.Idle();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Collect();
                _effectsService.OnCollect(() => Destroy(gameObject));
            }
        }

        public void Collect()
        {
            Status = CollectableStatusType.Unassigned;

            Debug.Log($"COLLECTED ITEM: <{Id} | {Status}>");
        }

        public void Spawn(int id, CollectableStatusType status)
        {
            Id = id;
            Status = status;
        }
    }
}