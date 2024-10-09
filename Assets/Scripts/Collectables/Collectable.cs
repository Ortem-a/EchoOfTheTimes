using EchoOfTheTimes.Interfaces;
using UnityEngine;

namespace EchoOfTheTimes.Collectables
{
    [RequireComponent(typeof(SphereCollider), typeof(CollectableEffectsService))]
    public class Collectable : MonoBehaviour, ICollectable
    {
        private SphereCollider _collider;
        private CollectableEffectsService _effectsService;
        private CollectableSpawner _spawner;

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
            Debug.Log($"COLLECTED: {name}");
        }
    }
}